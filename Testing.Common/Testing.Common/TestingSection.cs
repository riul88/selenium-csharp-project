using System.Configuration;

//Copyright (C) 2017 Raul Robledo <raul.robledo at acm.org>
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace Testing.Common
{
    public class TestingSection : ConfigurationSection
    {
        [ConfigurationProperty("baseUrl", DefaultValue = "", IsRequired = true)]
        public string BaseUrl
        {
            get
            {
                return (string)this["baseUrl"];
            }
            set
            {
                this["baseUrl"] = value;
            }
        }

        [ConfigurationProperty("browser", DefaultValue = "firefox", IsRequired = false)]
        public string Browser
        {
            get
            {
                return (string)this["browser"];
            }
            set
            {
                this["browser"] = value;
            }
        }

        [ConfigurationProperty("firefox")]
        public FirefoxElement Firefox
        {
            get
            {
                return (FirefoxElement)this["firefox"];
            }
            set
            { this["firefox"] = value; }
        }

        [ConfigurationProperty("remote")]
        public RemoteElement Remote
        {
            get
            {
                return (RemoteElement)this["remote"];
            }
            set
            { this["remote"] = value; }
        }

        [ConfigurationProperty("driver")]
        public DriverElement Driver
        {
            get
            {
                return (DriverElement)this["driver"];
            }
            set
            { this["driver"] = value; }
        }
    }

    public class RemoteElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true)]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }
    }

    public class DriverElement : ConfigurationElement
    {
        [ConfigurationProperty("implicitlyWait", DefaultValue = "0", IsRequired = false)]
        public int ImplicitlyWait
        {
            get
            {
                int value;
                if (int.TryParse(this["implicitlyWait"].ToString(), out value))
                {
                    return value;
                }
                return 0;
            }
            set
            {
                this["implicitlyWait"] = value.ToString();
            }
        }

        [ConfigurationProperty("setPageLoadTimeout", DefaultValue = "0", IsRequired = false)]
        public int SetPageLoadTimeout
        {
            get
            {
                int value;
                if (int.TryParse(this["setPageLoadTimeout"].ToString(), out value))
                {
                    return value;
                }
                return 0;
            }
            set
            {
                this["setPageLoadTimeout"] = value.ToString();
            }
        }

        [ConfigurationProperty("setScriptTimeout", DefaultValue = "0", IsRequired = false)]
        public int SetScriptTimeout
        {
            get
            {
                int value;
                if (int.TryParse(this["setScriptTimeout"].ToString(), out value))
                {
                    return value;
                }
                return 0;
            }
            set
            {
                this["setScriptTimeout"] = value.ToString();
            }
        }
   }

    public class FirefoxElement : ConfigurationElement
    {
        // Create a "path" attribute.
        [ConfigurationProperty("path", DefaultValue = "", IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
    }
}
