using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Text;


/******************Anfang JB*************/

namespace IntranetTG
{
    class FTPHandler
    {
        
        //Create the FTP request
        public FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://81.19.152.119/users.tg");

        /// <summary>
        /// This is the Download Function for FTP
        /// </summary>
        /// 

        string kk1 = "we";
        string kk2 = "r ";

        public FTPHandler()
        {
            //MessageBox.Show(kk1 + kk2 + kk3 + kk4 + kk5 + kk6 + kk7 + kk8 + kk9 + kk10 + kk11);
        }

        public bool down2(String Datei, string speicherort)
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://81.19.152.119/" + Datei);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("admin", "ADayToRemember2309");

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                //StreamReader reader = new StreamReader(responseStream);
                FileStream destination = File.Create(speicherort);
                CopyStream(responseStream, destination);

                //reader.Close();
                response.Close();
                responseStream.Close();
                destination.Close();

                return true;
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("nicht gefunden"))
                {
                    MessageBox.Show("Datei nicht gefunden");
                }
                else
                {
                    MessageBox.Show(ex.ToString());
                }
                return false;
            }
        }

        string kk7 = "is";
        string kk8 = " f";
        string kk9 = "ix";
        string kk10 = " z";
        string kk11 = "am";

        public void Down(String datei, Func<bool> target)
        {
            try
            {
                /*request = (FtpWebRequest)WebRequest.Create("ftp://81.19.152.119/"+datei);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential("admin", "RadeonHD7870");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                StreamWriter sw = new StreamWriter(datei, false);
                sw.Write(reader.ReadToEnd());
                sw.Close();
                reader.Close();
                response.Close();
                request.Abort();*/

                request = (FtpWebRequest)WebRequest.Create("ftp://81.19.152.119/" + datei);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential("admin", "ADayToRemember2309");
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse()) // wenn Fehler --> INTERNET !
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream destination = File.Create(datei))
                {
                    CopyStream(responseStream, destination);
                }

                if (target != null)
                {
                    target();
                }

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("nicht gefunden"))
                {
                    MessageBox.Show("Datei nicht gefunden");
                }
                else
                {
                    MessageBox.Show(ex.ToString());
                }
                return;
            }
        }

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
            }

        }


        /// <summary>Location of the file which is going to be uploaded
        /// This is the Upload function for FTP
        /// </summary>
        /// <param name="fileLoc"></param>
        /// 

        public bool up2(string file, string path)
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://81.19.152.119/" + path);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("admin", "ADayToRemember2309");

                // Copy the contents of the file to the request stream.
                //StreamReader sourceStream = new StreamReader(file);
                //byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                byte[] fileContents = File.ReadAllBytes(file);
                //sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
                return true;
            }
            catch 
        
            {

                MessageBox.Show("Es kann nicht auf die Datei zugegriffen werden, da sie in einem anderen Programm geöffnet ist!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
        }

        string kk3 = "sc";
        string kk4 = "hm";
        string kk5 = "us";
        string kk6 = "t ";

        public void Up(String fileLoc)
        {
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("admin", "RadeonHD7870");
            WebResponse response = request.GetResponse();
            FileStream fs = new FileStream(fileLoc, FileMode.Open);
            byte[] fileContents = new byte[fs.Length];
            fs.Read(fileContents, 0, Convert.ToInt32(fs.Length));
            fs.Flush();
            fs.Close();
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
            request.Abort();    
        }

        public void delete(string file)
        {
            try
            {
                FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create("ftp://81.19.152.119/" + "/home/.sites/33/site5/web/intranet/app/webroot" + file);
                requestFileDelete.Credentials = new NetworkCredential("admin", "ADayToRemember2309");
                requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;

                FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void createPathForClient(string id)
        {
            try
            {
                WebRequest request = WebRequest.Create("ftp://81.19.152.119/" + "/home/.sites/33/site5/web/intranet/app/webroot/data/clients/" + id);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential("admin", "ADayToRemember2309");
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    if (resp.StatusCode.ToString() != "PathnameCreated")
                    {
                        MessageBox.Show("Bitte melden Sie dem Administrator, dass bei diesem Klient (Client Number: " + id + ") der FTP-Pfad händisch angelegt werden muss.");
                    }
                }
                createPathForClient2(id);
                createPathForClient3(id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void createPathForClient2(string id)
        {
            try
            {
                WebRequest request = WebRequest.Create("ftp://81.19.152.119/" + "/home/.sites/33/site5/web/intranet/app/webroot/data/clients/" + id + "/photos/");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential("admin", "ADayToRemember2309");
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    if (resp.StatusCode.ToString() != "PathnameCreated")
                    {
                        MessageBox.Show("Bitte melden Sie dem Administrator, dass bei diesem Klient (Client Number: " + id + ") der FTP-Pfad (/photos) händisch angelegt werden muss.");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void createPathForClient3(string id)
        {
            try
            {
                WebRequest request = WebRequest.Create("ftp://81.19.152.119/" + "/home/.sites/33/site5/web/intranet/app/webroot/data/clients/" + id + "/documents/");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential("admin", "ADayToRemember2309");
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    if (resp.StatusCode.ToString() != "PathnameCreated")
                    {
                        MessageBox.Show("Bitte melden Sie dem Administrator, dass bei diesem Klient (Client Number: " + id + ") der FTP-Pfad (/documents) händisch angelegt werden muss.");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
/******************Ende JB*************/