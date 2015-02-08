namespace SolutionStartPage.Controls.SolutionsPart
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Models;

    public class SolutionPageModel
    {
        /////////////////////////////////////////////////////////
        #region Constants

        private const string _SETTINGS_FILE_NAME_2013 = "VS2013_SolutionStartPage.settings";
        private const int _MAXIMUM_RETRIES = 15;

        #endregion

        /////////////////////////////////////////////////////////
        #region Fields

        private readonly XmlSerializer _serializer;
        private readonly string _settingsFilePath;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        public SolutionPageModel()
        {
            _serializer = new XmlSerializer(typeof(SolutionPageConfiguration));
            _settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                _SETTINGS_FILE_NAME_2013);
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Public Methods

        public SolutionPageConfiguration LoadConfiguration()
        {
            if (!File.Exists(_settingsFilePath))
                return new SolutionPageConfiguration();

            for (var i = 0; i <= _MAXIMUM_RETRIES; i++)
            {
                try
                {
                    using (var reader = new StreamReader(_settingsFilePath))
                        return (SolutionPageConfiguration)_serializer.Deserialize(reader);
                }
                catch (Exception)
                {
                    if (i >= _MAXIMUM_RETRIES)
                        return new SolutionPageConfiguration();
                }
            }
            return new SolutionPageConfiguration();
        }

        public async Task SaveConfiguration(SolutionPageConfiguration groups)
        {
            for (var i = 0; i <= _MAXIMUM_RETRIES; i++)
            {
                try
                {
                    using (var writer = new StreamWriter(_settingsFilePath))
                    {
                        _serializer.Serialize(writer, groups);
                        return;
                    }
                }
                catch (Exception)
                {
                    if (i >= _MAXIMUM_RETRIES)
                        return;
                }
                await Task.Delay(200);
            }
        }

        #endregion
    }
}