namespace Event_Booking_System_API.EventService.Helpers
{
    public static class SaveImages
    {
        public static async Task<string> SaveEventImageAsync(this IFormFile image, string Location, IWebHostEnvironment webHostEnvironment)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Invalid image file");

            // Create the EventImages directory if it doesn't exist
            var uploadsFolder = Path.Combine("wwwroot", Location);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique filename
            string uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // Return the relative URL path
            return $"/{Location}/{uniqueFileName}";
        }
    }
}
