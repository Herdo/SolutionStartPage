namespace SolutionStartPage.Shared.Views.SolutionPageView
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Models;

    public abstract class SolutionPageModelBase : ISolutionPageModel
    {
        /////////////////////////////////////////////////////////
        #region Constants

        private const int _MAXIMUM_RETRIES = 15;

        #endregion

        /////////////////////////////////////////////////////////
        #region Fields

        private readonly XmlSerializer _serializer;
        private readonly string _settingsFilePath;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        protected SolutionPageModelBase(string settingsFileName)
        {
            _serializer = new XmlSerializer(typeof(SolutionPageConfiguration));
            _settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), settingsFileName);
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region ISolutionPageModel Member

        SolutionPageConfiguration ISolutionPageModel.LoadConfiguration()
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

        async Task ISolutionPageModel.SaveConfiguration(SolutionPageConfiguration groups)
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