using Lucene.Net.Documents;
using Newtonsoft.Json;

namespace Hydra.Domain.Models.Lucene;

public record DownloadDocument(string Title, string Source, DateTime Uploaded) : DocumentBase(Generation.GenerateId(Title, Source)), IDisposable
{
    public void Dispose() { }
    
    public string? Size { get; set; }
    public List<string>? Uris { get; set; }
    public DateTime UploadDate { get; set; }

    public static explicit operator Document(DownloadDocument d)
    {
        var doc = new Document();
        
        doc.Add(new StringField("Id", d.Id, Field.Store.YES));
        doc.Add(new TextField("Title", d.Title, Field.Store.YES));
        doc.Add(new StringField("Source", d.Source, Field.Store.YES));
        doc.Add(new StoredField("Uris", JsonConvert.SerializeObject(d.Uris)));
        doc.Add(new StoredField("Size", d.Size));
        doc.Add(new StoredField("UploadDate", d.UploadDate.ToString("O")));
        
        return doc;
    }
}