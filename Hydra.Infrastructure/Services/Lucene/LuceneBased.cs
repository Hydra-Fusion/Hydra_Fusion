using Hydra.Domain.Models.Lucene;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Directory = System.IO.Directory;

namespace Hydra.Infrastructure.Services.Lucene;

public abstract class LuceneBased<T> where T : DocumentBase, IDisposable
{
    public string Name { get; set; }
    public string BasePath { get; set; }
    
    protected IndexWriter _writer;

    public LuceneBased(string name, string basePath)
    {
        Name = name;
        BasePath = basePath;
        
        InitializeWriter();
    }
    
    private void InitializeWriter()
    {
        if(!Directory.Exists(BasePath))
            Directory.CreateDirectory(BasePath);
        
        var directory = FSDirectory.Open(BasePath);
        var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
        var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
        _writer = new IndexWriter(directory, config);
    }

    public async Task AddOrUpdateAsync(T item, Func<T, Document> func)
    {
        if (_writer == null) throw new InvalidOperationException("IndexWriter não inicializado.");
        
        var doc = func(item);
        
        _writer.UpdateDocument(new Term("Id", item.Id), doc);
        
        _writer.Commit();
    }
    
    public async Task AddOrUpdateBulkAsync(ICollection<T> items, Func<T, Document> func)
    {
        if (_writer == null)
            throw new InvalidOperationException("IndexWriter not initialized.");

        if (items == null || items.Count == 0)
            return;

        // 1️⃣ Extrai todos os documentos
        var docs = new List<Document>(items.Count);
        var ids = new List<Term>(items.Count);

        foreach (var item in items)
        {
            docs.Add(func(item));
            ids.Add(new Term("Id", item.Id));
        }

        // 2️⃣ Deleta todos de uma vez (muito mais rápido)
        _writer.DeleteDocuments(ids.ToArray());

        // 3️⃣ Adiciona tudo de uma vez
        _writer.AddDocuments(docs);

        // 4️⃣ Commit único
        await Task.Run(() => _writer.Commit());
    }


    public async Task DeleteAsync()
    {
        if (_writer == null) throw new InvalidOperationException("IndexWriter não inicializado.");
        
        _writer.Dispose();
        _writer = null;

        if (Directory.Exists(BasePath))
            Directory.Delete(BasePath, true);
    }
    
    public void Dispose()
    {
        _writer.Commit();
        _writer.Dispose();
    }
}