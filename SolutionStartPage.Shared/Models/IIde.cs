namespace SolutionStartPage.Shared.Models
{
    public interface IIde
    {
        string Edition { get; }

        void OpenSolution(string path);
    }
}