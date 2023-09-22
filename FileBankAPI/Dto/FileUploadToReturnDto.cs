namespace FileBankAPI.Dto
{
    public class FileUploadToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileUrl {  get; set; }
        public string FileType { get; set; }
        public double FileSize { get; set; }
    }
}
