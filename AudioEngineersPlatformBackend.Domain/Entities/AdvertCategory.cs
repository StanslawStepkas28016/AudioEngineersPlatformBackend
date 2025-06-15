namespace AudioEngineersPlatformBackend.Domain.Entities;

public class AdvertCategory
{
    public Guid IdAdvertCategory { get; set; }
    public string CategoryName { get; set; }
    public ICollection<Advert> Adverts { get; set; }

    public AdvertCategory(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            throw new ArgumentException("Category name cannot be null or whitespace.", nameof(categoryName));
        }

        IdAdvertCategory = Guid.NewGuid();
        CategoryName = categoryName;
    }
}