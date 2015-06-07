namespace SolutionStartPage.Shared.Views
{
    public interface IAppControlHost
    {
        /// <summary>
        /// Gets or sets the DataContext for the whole start page.
        /// </summary>
        object DataContext { get; set; }
    }
}