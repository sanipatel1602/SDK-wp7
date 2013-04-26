using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;

namespace QuickBlox.SuperSample.Model
{
    public class Message : INotifyPropertyChanged
    {
        private bool isLoad;
        /// <summary>
        /// The result of parcing procedure (for message response)
        /// </summary>
        public bool IsLoad
        {
            get { return isLoad; }
            set
            {
                if (isLoad == value) return;
                isLoad = value;
                RaisePropertyChanged("IsLoad");
            }
        }

        private string text;
        /// <summary>
        /// Message text
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value) return;
                text = value;
                RaisePropertyChanged("Text");
            }
        }

        private int to;
        /// <summary>
        /// (Receiver)
        /// The ID of User for whom message was sent  
        /// Can be null (for main chat, location, etc...).
        /// Look at the Message Type
        /// </summary>
        public int To
        {
            get { return to; }
            set
            {
                if (to == value) return;
                to = value;
                RaisePropertyChanged("To");
            }
        }

        private int from;
        /// <summary>
        /// The ID of User who has send the message
        /// </summary>
        public int From
        {
            get { return from; }
            set
            {
                if (from == value) return;
                from = value;
                RaisePropertyChanged("From");
            }
        }

        private DateTime date;
        /// <summary>
        /// Date of message
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date == value) return;
                date = value;
                RaisePropertyChanged("Date");
            }
        }

        public MessageType type;
        /// <summary>
        /// The message type
        /// </summary>
        public MessageType Type
        {
            get { return type; }
            private set
            {
                if (type == value) return;
                type = value;
                RaisePropertyChanged("Type");
            }
        }

        public decimal latitude;
        /// <summary>
        /// Latitude
        /// </summary>
        public decimal Latitude
        {
            get { return latitude; }
            set
            {
                if (latitude == value) return;
                latitude = value;
                RaisePropertyChanged("Latitude");
            }
        }

        public decimal longitude;
        /// <summary>
        /// Longitude
        /// </summary>
        public decimal Longitude
        {
            get { return longitude; }
            set
            {
                if (longitude == value) return;
                longitude = value;
                RaisePropertyChanged("Longitude");
            }
        }

        /// <summary>
        /// Message constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="Text"></param>
        public Message(MessageType type, int From, int To, string Text)
        {
            this.Type = type;
            this.Text = string.IsNullOrEmpty(Text)? string.Empty: Text.Trim();
            this.From = From;
            this.To = To;
 
        }


        private string ToBase64(string str)
        {
            return System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(str));
        }

        private string FromBase64(string str)
        {
            byte[] tr = System.Convert.FromBase64String(str);
           // string result = System.Text.UTF8Encoding.UTF8.GetString(tr);
            UTF8Encoding encoder = new UTF8Encoding();
            string result = encoder.GetString(tr, 0, tr.Length);
            return result;

        }

        public Message(string Scheme)
        {
            if (string.IsNullOrEmpty(Scheme))
                this.IsLoad = false;

           // Scheme = Scheme.Replace("&lt;", "<").Replace("&gt;", ">");
            this.ParseMessage(this.FromBase64(Scheme.Replace(' ','+')));
            
        }

        private void ParseMessage(string xml)
        {
            try
            {
                XElement xmlResult = XElement.Parse(xml);

                switch (xmlResult.Element("t").Value)
                {
                    case "1":
                        {
                            this.Type = MessageType.Location;
                            break;
                        }
                    case "2":
                        {
                            this.Type = MessageType.Message;
                            break;
                        }
                    case "3":
                        {
                            this.Type = MessageType.PrivateMessage;
                            break;
                        }
                    case "4":
                        {
                            this.Type = MessageType.VisitedPlaces;
                            break;
                        }
                    default:
                        {
                            this.IsLoad = false;
                            return;
                        }
                }

                if (this.Type == MessageType.PrivateMessage)
                {
                   // this.From = int.Parse(xmlResult.Element("from").Value);
                    this.To = int.Parse(xmlResult.Element("to").Value);
                }
                
                this.Text = xmlResult.Element("te").Value;
                this.IsLoad = true;
            }
            catch
            {
                this.IsLoad = false;
            }
        }


        public override string ToString()
        {

            StringBuilder result = new StringBuilder();
            result.Append("<m>");
            result.Append("<t>");

            switch (this.Type)
            {
                case MessageType.Location:
                    {
                        result.Append("1");
                        break;
                    }
                case MessageType.Message:
                    {
                        result.Append("2");
                        break;
                    }
                case MessageType.PrivateMessage:
                    {
                        result.Append("3");
                        break;
                    }
                case MessageType.VisitedPlaces:
                    {
                        result.Append("4");
                        break;
                    }
            }

            result.Append("</t>");

            if (this.Type == MessageType.PrivateMessage)
            {

                result.Append("<to>");
                result.Append(this.To);
                result.Append("</to>");

               /* result.Append("<from>");
                result.Append(this.From);
                result.Append("</from>");*/
            }

            result.Append("<te>");
            result.Append(this.Text);
            result.Append("</te>");
            result.Append("</m>");


            return this.ToBase64(result.ToString());
        }

       

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
