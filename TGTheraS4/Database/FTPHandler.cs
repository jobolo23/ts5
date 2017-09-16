using System;
using System.IO;
using System.Net;
using System.Windows;

namespace IntranetTG
{
    internal class FtpHandler
    {
        private const string FtpUrl = "ftp://ftp.epplicator.com/TS5/";
        private readonly NetworkCredential _ftpCred = new NetworkCredential("u79279720", "catter?12");

        private FtpWebRequest _request;

        public FtpHandler()
        {

        }

        public bool DownloadFile(string datei, string speicherort)
        {
            try
            {
                var request = (FtpWebRequest) WebRequest.Create(FtpUrl + datei);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = _ftpCred;

                var response = (FtpWebResponse) request.GetResponse();

                var responseStream = response.GetResponseStream();
                var destination = File.Create(speicherort);
                CopyStream(responseStream, destination);

                response.Close();
                responseStream?.Close();
                destination.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().Contains("nicht gefunden") ? "Datei nicht gefunden" : ex.ToString());
                return false;
            }
        }



        // part of the old update routine...

        /*
        public void DownloadFile(string datei, Func<bool> target)
        {
            try
            {
                _request = (FtpWebRequest) WebRequest.Create(FtpUrl + datei);
                _request.Method = WebRequestMethods.Ftp.DownloadFile;
                _request.Credentials = _ftpCred;
                using (var response = (FtpWebResponse) _request.GetResponse()) // wenn Fehler --> INTERNET !
                using (var responseStream = response.GetResponseStream())
                using (var destination = File.Create(datei))
                {
                    CopyStream(responseStream, destination);
                }

                target?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().Contains("nicht gefunden") ? "Datei nicht gefunden" : ex.ToString());
            }
        }
        */

        private static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[32768];
            while (true)
            {
                var read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
            }
        }

        public bool UploadFile(string file, string path)
        {
            try
            {
                var request = (FtpWebRequest) WebRequest.Create(FtpUrl + path);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = _ftpCred;

                var fileContents = File.ReadAllBytes(file);

                request.ContentLength = fileContents.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                var response = (FtpWebResponse) request.GetResponse();

                response.Close();
                return true;
            }
            catch
            {
                MessageBox.Show(
                    "Es kann nicht auf die Datei zugegriffen werden, da sie in einem anderen Programm geöffnet ist!",
                    "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }


        // No usages????

        /*
        public void Up(string fileLoc)
        {
            _request.Method = WebRequestMethods.Ftp.UploadFile;
            _request.Credentials = _ftpCred;
            var response = _request.GetResponse();
            var fs = new FileStream(fileLoc, FileMode.Open);
            var fileContents = new byte[fs.Length];
            fs.Read(fileContents, 0, Convert.ToInt32(fs.Length));
            fs.Flush();
            fs.Close();
            var requestStream = _request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
            _request.Abort();
        }
        */

        public void DeleteFile(string file)
        {
            try
            {
                var requestFileDelete = (FtpWebRequest) WebRequest.Create(FtpUrl + file);
                requestFileDelete.Credentials = _ftpCred;
                requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;

                var responseFileDelete = (FtpWebResponse) requestFileDelete.GetResponse();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void CreatePathForClient(string id)
        {
            try
            {
                var request = WebRequest.Create($"{FtpUrl}data/clients/{id}");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = _ftpCred;
                using (var resp = (FtpWebResponse) request.GetResponse())
                {
                    if (resp.StatusCode.ToString() != "PathnameCreated")
                    {
                        MessageBox.Show("Bitte melden Sie dem Administrator, dass bei diesem Klient (Client Number: " +
                                        id + ") der FTP-Pfad händisch angelegt werden muss.");
                    }
                }
                CreatePhotoPathForClient(id);
                CreateDocumentsPathForClient(id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void CreatePhotoPathForClient(string id)
        {
            try
            {
                var request = WebRequest.Create($"{FtpUrl}data/clients/{id}/photos/");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = _ftpCred;
                using (var resp = (FtpWebResponse) request.GetResponse())
                {
                    if (resp.StatusCode.ToString() != "PathnameCreated")
                    {
                        MessageBox.Show("Bitte melden Sie dem Administrator, dass bei diesem Klient (Client Number: " +
                                        id + ") der FTP-Pfad (/photos) händisch angelegt werden muss.");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void CreateDocumentsPathForClient(string id)
        {
            try
            {
                var request = WebRequest.Create($"{FtpUrl}data/clients/{id}/documents/");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = _ftpCred;
                using (var resp = (FtpWebResponse) request.GetResponse())
                {
                    if (resp.StatusCode.ToString() != "PathnameCreated")
                    {
                        MessageBox.Show("Bitte melden Sie dem Administrator, dass bei diesem Klient (Client Number: " +
                                        id + ") der FTP-Pfad (/documents) händisch angelegt werden muss.");
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