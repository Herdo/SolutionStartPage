namespace SolutionStartPage.Shared.Models
{
    using System.Linq;
    using System.Xml.Serialization;
    using Views.SolutionPageView;

    [XmlRoot]
    public class SolutionPageConfiguration
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private int _columns;

        #endregion

        /////////////////////////////////////////////////////////
        #region Properties

        [XmlElement]
        public int Columns
        {
            get => _columns;
            set
            {
                if (value == _columns) return;
                if (value < 1)
                    _columns = 1;
                else if (value > 3)
                    _columns = 3;
                else
                    _columns = value;
            }
        }

        [XmlElement]
        public bool DisplayFolders { get; set; }

        [XmlElement]
        public bool DisplayIcons { get; set; }

        [XmlElement]
        public bool DisplaySeparator { get; set; }

        [XmlArray("SolutionGroups")]
        [XmlArrayItem("SolutionGroup")]
        public SolutionGroup[] SolutionGroups { get; set; }

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionPageConfiguration()
        {
            Columns = 3;
            DisplayFolders = true;
            DisplayIcons = true;
            DisplaySeparator = true;
            SolutionGroups = new SolutionGroup[0];
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Public Methods

        public SolutionPageConfiguration ApplyViewModel(ISolutionPageViewModel vm)
        {
            Columns = vm.Columns;
            DisplayFolders = vm.DisplayFolders;
            DisplayIcons = vm.DisplayIcons;
            DisplaySeparator = vm.DisplaySeparator;
            SolutionGroups = vm.SolutionGroups.ToArray();

            return this;
        }

        #endregion
    }
}