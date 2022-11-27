namespace WSBLearn.Application.Extensions
{
    public static class FileValidation
    {
        public static bool IsUnsupportedImageFileExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            var list = new List<string> { ".jpg", ".png", ".svg" };
            return !list.Contains(ext);
        }
    }
}
