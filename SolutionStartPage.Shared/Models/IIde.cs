namespace SolutionStartPage.Shared.Models
{
    public interface IIde
    {
        string Edition { get; }

        object IdeAccess { set; }

        void OpenSolution(string path);
    }
}