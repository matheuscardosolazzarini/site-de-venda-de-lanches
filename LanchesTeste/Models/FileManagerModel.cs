namespace LanchesTeste.Models
{
    public class FileManagerModel
    {
        public FileInfo[] Files { get; set; }
        public IFormFile IFormeFile { get; set; }
        public List<IFormFile> IFormeFiles { get; set;}
        public string PathImagesProduto { get; set; }
    }
}
