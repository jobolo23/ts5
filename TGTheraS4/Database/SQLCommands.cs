using IntranetTG.Objects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using TGTheraS4.Database.Objects;
using TGTheraS4.Objects;
using TGTheraS4.Database_SQLite;

namespace IntranetTG
{
    /*******************************************
     * All of the return have a '$' as inline Delimitter
     * and a '%' as Line Break 
     * 
     * *****************************************/

    /************Anfang JB***************/ 
    public class SQLCommands 
    {
        //public MySqlConnection myConnection = new MySqlConnection("uid=ultra;" + "pwd=radeonhd7870;server=81.19.152.119;" + "database=intranetTest; Convert Zero Datetime=True"); //Test-Server
        private readonly MySqlConnection _myConnection; //Main-Server

        public SQLCommands(MySqlConnectionInformation connectionData)
        {
            _myConnection = new MySqlConnection($"uid={connectionData.Username};pwd={connectionData.Password};server={connectionData.Host};database={connectionData.Database}; Convert Zero Datetime=True");
        }

        /// <summary>
        /// The Children get connected to the Houses
        /// </summary>
        /// <param name="wg">Name of the house of which u need the Children</param>
        /// <returns>Firstname and lastname of the Kids</returns>
        /// 

        public List<NewestDokus> getDokusByWgsAndDate(string[] wgs, string dateFrom)
        {
            List<NewestDokus> list = new List<NewestDokus>();
            string sql_wgs = "";
            for (int i = 0; i < wgs.Length; i++) {
                if ((wgs.Length - i) > 1)
                {
                    sql_wgs = sql_wgs + "s.name = '" + wgs[i] + "' OR ";
                }
                else
                {
                    sql_wgs = sql_wgs + "s.name = '" + wgs[i] + "'";
                }
            }
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("SELECT c.firstname cfirstname, c.lastname clastname, d.created dcreated, d.for_day, u.firstname ufirstname, u.lastname ulastname, s.name sname FROM clientsdailydocs d JOIN clients c ON c.id = d.client_id JOIN clientstoservices cs ON c.id = cs.client_id JOIN services s ON cs.service_id = s.id JOIN users u ON u.id = d.createuser_id WHERE (" + sql_wgs + ") AND d.created > '" + dateFrom + "' ORDER BY d.created DESC", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    
                    string nn1 = "";
                    string nn2 = "";
                    if (myReader["cfirstname"].ToString().Contains(" "))
                    {
                        nn1 = myReader["cfirstname"].ToString().Replace(' ', ',');
                    }
                    else
                    {
                        nn1 = myReader["cfirstname"].ToString();
                    }
                    if (myReader["clastname"].ToString().Contains(" "))
                    {
                        nn2 = myReader["clastname"].ToString().Replace(' ', ',');
                    }
                    else
                    {
                        nn2 = myReader["clastname"].ToString();
                    }
                    String name = nn1 + " " + nn2;
                    String tag = myReader["for_day"].ToString().Split(' ')[0];
                    String created = myReader["dcreated"].ToString().Split(' ')[0];
                    String wg = myReader["sname"].ToString();
                    String ersteller = myReader["ufirstname"].ToString() + " " + myReader["ulastname"].ToString();
                    String art = "Tagesdokumentation";
                    list.Add(new NewestDokus(name, tag, wg, ersteller, art, created, tag + " - " + myReader["dcreated"].ToString()));
                }

                myReader.Close();




                myCommand = new MySqlCommand("SELECT c.firstname cfirstname, c.lastname clastname, r.created rcreated, r.name rname, r.art rart, u.firstname ufirstname, u.lastname ulastname, s.name sname FROM clientsreports r JOIN clients c ON c.id = r.client_id JOIN clientstoservices cs ON c.id = cs.client_id JOIN services s ON cs.service_id = s.id JOIN users u ON u.id = r.createuser_id WHERE (" + sql_wgs + ") AND r.created > '" + dateFrom + "' ORDER BY r.created DESC", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    string nn1 = "";
                    string nn2 = "";
                    if (myReader["cfirstname"].ToString().Contains(" "))
                    {
                        nn1 = myReader["cfirstname"].ToString().Replace(' ', ',');
                    }
                    else
                    {
                        nn1 = myReader["cfirstname"].ToString();
                    }
                    if (myReader["clastname"].ToString().Contains(" "))
                    {
                        nn2 = myReader["clastname"].ToString().Replace(' ', ',');
                    }
                    else
                    {
                        nn2 = myReader["clastname"].ToString();
                    }
                    string name = nn1 + " " + nn2;


                    string tag = myReader["rname"].ToString();
                    string created = myReader["rcreated"].ToString().Split(' ')[0];
                    string wg = myReader["sname"].ToString();
                    string ersteller = myReader["ufirstname"].ToString() + " " + myReader["ulastname"].ToString();
                    string art = "Bericht";
                    switch (myReader["rart"].ToString())
                    {
                        case "0":
                            art = "Telefonat";
                            break;
                        case "1":
                            art = "Vorfallsprotokoll";
                            break;
                        case "2":
                            art = "Gesprächsprotokoll";
                            break;
                        case "3":
                            art = "Fallverlaufsgespräch";
                            break;
                        case "4":
                            art = "Jahresbericht";
                            break;
                        case "5":
                            art = "Zwischenbericht";
                            break;
                        default:
                            art = "Bericht";
                            break;
                    }
                    list.Add(new NewestDokus(name, tag, wg, ersteller, art, created, tag + " - " + myReader["rcreated"].ToString()));
                    
                }

                myReader.Close();




                myCommand = new MySqlCommand("SELECT c.firstname cfirstname, c.lastname clastname, r.created rcreated, r.name rname, r.art rart, u.firstname ufirstname, u.lastname ulastname, s.name sname FROM clientsfvgs r JOIN clients c ON c.id = r.client_id JOIN clientstoservices cs ON c.id = cs.client_id JOIN services s ON cs.service_id = s.id JOIN users u ON u.id = r.createuser_id WHERE (" + sql_wgs + ") AND r.created > '" + dateFrom + "' ORDER BY r.created DESC", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    string nn1 = "";
                    string nn2 = "";
                    if (myReader["cfirstname"].ToString().Contains(" "))
                    {
                        nn1 = myReader["cfirstname"].ToString().Replace(' ', ',');
                    }
                    else
                    {
                        nn1 = myReader["cfirstname"].ToString();
                    }
                    if (myReader["clastname"].ToString().Contains(" "))
                    {
                        nn2 = myReader["clastname"].ToString().Replace(' ', ',');
                    }
                    else
                    {
                        nn2 = myReader["clastname"].ToString();
                    }
                    string name = nn1 + " " + nn2;
                    string tag = myReader["rname"].ToString();
                    string created = myReader["rcreated"].ToString().Split(' ')[0];
                    string wg = myReader["sname"].ToString();
                    string ersteller = myReader["ufirstname"].ToString() + " " + myReader["ulastname"].ToString();
                    string art = "Bericht";
                    switch (myReader["rart"].ToString())
                    {
                        case "0":
                            art = "Telefonat";
                            break;
                        case "1":
                            art = "Vorfallsprotokoll";
                            break;
                        case "2":
                            art = "Gesprächsprotokoll";
                            break;
                        case "3":
                            art = "Fallverlaufsgespräch";
                            break;
                        case "4":
                            art = "Jahresbericht";
                            break;
                        case "5":
                            art = "Zwischenbericht";
                            break;
                        default:
                            art = "Bericht";
                            break;
                    }
                    list.Add(new NewestDokus(name, tag, wg, ersteller, art, created, tag + " - " + myReader["rcreated"].ToString()));

                }

                myReader.Close();



                list.Sort(delegate(NewestDokus o1, NewestDokus o2)
                {
                    return o1.created.CompareTo(o2.created);
                });
                list.Reverse();
                return list;
            }
            catch 
            {
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        /// <summary>
        /// The Children get connected to the Houses
        /// </summary>
        /// <param name="wg">Name of the house of which u need the Children</param>
        /// <returns>Firstname and lastname of the Kids</returns>

        public string WgToClients(string wg)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                //----------Begin DC----------
                /*MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname,s.name from clients c" +
                    " join clientstoservices cs on c.id = cs.client_id" +
                    " join services s on cs.service_id = s.id" +
                    " where s.name = '" + wg + "' " +
                    "and c.leaving is null", myConnection);*/
                MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname,s.name from clients c" +
                    " join clientstoservices cs on c.id = cs.client_id" +
                    " join services s on cs.service_id = s.id" +
                    " where s.name = '" + wg + "' " +
                    "and c.leaving is null order by lastname, s.name ASC", _myConnection);
                //----------End DC----------
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["firstname"].ToString() + "$";
                    ret += myReader["lastname"].ToString();
                    ret += "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public string WgToClientsAllClients(string wg)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                //----------Begin DC----------
                /*MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname,s.name from clients c" +
                    " join clientstoservices cs on c.id = cs.client_id" +
                    " join services s on cs.service_id = s.id" +
                    " where s.name = '" + wg + "'", myConnection);*/
                MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname,s.name from clients c" +
                    " join clientstoservices cs on c.id = cs.client_id" +
                    " join services s on cs.service_id = s.id" +
                    " where s.name = '" + wg + "' order by lastname, s.name ASC", _myConnection);
                //----------End DC----------
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["firstname"].ToString() + "$";
                    ret += myReader["lastname"].ToString();
                    ret += "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public string WgToClientsAllClients_notleft(string wg)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                //----------Begin DC----------
                /*MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname,s.name from clients c" +
                    " join clientstoservices cs on c.id = cs.client_id" +
                    " join services s on cs.service_id = s.id" +
                    " where s.name = '" + wg + "' and leaving is not null", myConnection);*/
                MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname,s.name from clients c" +
                    " join clientstoservices cs on c.id = cs.client_id" +
                    " join services s on cs.service_id = s.id" +
                    " where s.name = '" + wg + "' and leaving is not null order by lastname, s.name ASC", _myConnection);
                //----------End DC----------
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["firstname"].ToString() + "$";
                    ret += myReader["lastname"].ToString();
                    ret += "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public string getClientdoku(int id)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select client_id, created, modified, createuser_id, lastuser_id, title, path, filesize from clientsdocuments where client_id = " + id, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["client_id"].ToString() + "$";
                    ret += myReader["created"].ToString() + "$";
                    ret += myReader["modified"].ToString() + "$";
                    ret += myReader["createuser_id"].ToString() + "$";
                    ret += myReader["lastuser_id"].ToString() + "$";
                    ret += myReader["title"].ToString() + "$";
                    ret += myReader["path"].ToString() + "$";
                    ret += myReader["filesize"].ToString();
                    ret += "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void addwikirate(Feedback feedback)
        {
            try
            {
                //if exists
                _myConnection.Open();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("update feedback set rate = " + feedback.rating + " where userId = " + feedback.userid.ToString() + " ;", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch 
            {
                MessageBox.Show("Das Dokument existiert nicht mehr","Fehler!", MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getWikiDocs()
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select id, created, modified, createuser_id, lastuser_id, title, path, rate from wiki",_myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["id"].ToString() + "$";
                    ret += myReader["created"].ToString() + "$";
                    ret += myReader["modified"].ToString() + "$";
                    ret += myReader["createuser_id"].ToString() + "$";
                    ret += myReader["lastuser_id"].ToString() + "$";
                    ret += myReader["title"].ToString() + "$";
                    ret += myReader["path"].ToString() + "$";
                    ret += myReader["rate"].ToString() + "$";
                    ret += "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getMailCredetials(string id)
        {
            return getData("users", new string[] { "s.email_user", "s.email_password" }, new string[] { "email_user", "email_password" }, "where s.id = '" + id + "'");
        }

        public string getMailCredetials_World4U(string id)
        {
            return getData("users", new string[] { "s.email_address", "s.email_password" }, new string[] { "email_address", "email_password" }, "where s.id = '" + id + "'");
        }

        public string getDropbox(string id)
        {
            return getData("users", new string[] { "s.email_address", "s.Dropboxpw" }, new string[] { "email_address", "Dropboxpw" }, "where s.id = '" + id + "'");
        }

        public string getMailCredentials_Gmail(string id)
        {
            return getData("users", new string[] { "s.gmail_address", "s.gmail_password" }, new string[] { "gmail_address", "gmail_password" }, "where s.id = '" + id + "'");
        }

        #region Medication
        /// <summary>
        /// Children get connected to their Medications
        /// </summary>
        /// <param name="vorname">Firstname of Child u want MED</param>
        /// <param name="nachname">Lastname of Child u want MED</param>

        public string clientToMedication(string vorname, string nachname)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname,m.name from clients c" +
                    " join clientsmedications cm on c.id = cm.client_id" +
                    " join medicaments m on cm.medicament_id = m.id" +
                    " where c.firstname = '" + vorname + "' and c.lastname = '" + nachname + "' " +
                    "and cm.to >= curdate()", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    ret += myReader["name"].ToString();
                    ret += "%";
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        #endregion

        #region Shouts

        /// <summary>
        /// Gets all shout starts with the new ones
        /// </summary>
        public string getShouts()
        {
            string ret = "";

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select s.message, s.created, u.firstname, u.lastname from shouts s join users u on s.createuser_id = u.id" +
                " order by created DESC LIMIT 0, 30", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["message"].ToString() + "$";
                    ret += myReader["created"].ToString() + "$";
                    ret += myReader["firstname"].ToString() + " ";
                    ret += myReader["lastname"].ToString();
                    ret += "%";
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public void setShout(string shout, string id)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("insert into shouts (created,createuser_id,message) values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "','" + id + "','" + shout + "')", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            finally
            {
                _myConnection.Close();
            }
        }

        #endregion

        #region Doku

        // Doku


        //info an Anna:

        //hier bitte die daten im folgenden Format returnen:        "TextKörperlich%TextSchlisch%TextPsychisch%TextAußenkontakt%TextPflichten"
        // weiters bitte leere felder als "" übergeben und trotzdem mit % trennen danke schön 
        internal string getDoku(string p, DateTime dateTime)
        {
            string ret = "";

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                string[] temp = p.Split(' ');
                MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname, d.content_bodily, " +
                    "d.content_school, d.content_psychic, d.content_external_contact, d.content_responsibilities, d.createuser_id " +
                    "from clients c join clientsdailydocs d on c.id = d.client_id where c.firstname ='"
                    + temp[0].Replace(',', ' ') + "' and c.lastname ='" + temp[1].Replace(',', ' ') + "' and d.for_day between'" + dateTime.ToString("yyyy-MM-dd") + " 00:00' and '" + dateTime.ToString("yyyy-MM-dd") + " 23:59'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["content_bodily"].ToString() + "$";
                    ret += myReader["content_school"].ToString() + "$";
                    ret += myReader["content_psychic"].ToString() + "$";
                    ret += myReader["content_external_contact"].ToString() + "$";
                    ret += myReader["content_responsibilities"].ToString() + "$";
                    ret += myReader["createuser_id"].ToString() + "$";
                    ret += "%";
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getMedication(string kid, DateTime dt)
        {
            string ret = "";
            string[] temp = kid.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night, cmc.morning mo,cmc.midday mi,cmc.evening ev,cmc.night ni, cmc.created crea from clients c" +
                    " join clientsmedications cm on c.id = cm.client_id" +
                    " join medicaments m on cm.medicament_id = m.id" +
                    " join clientsmedicationsconfirmations cmc on cmc.clientsmedication_id = cm.id" +
                    " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" + temp[1].Replace(',', ' ') + "' " +
                    "and cm.to >= '" + dt.ToString("yyyy-MM-dd") + "' and cmc.for_day ='" + dt.ToString("yyyy-MM-dd") + "'", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    ret += myReader["name"].ToString() + "$";
                    ret += myReader["morning"].ToString() + "$";
                    ret += myReader["midday"].ToString() + "$";
                    ret += myReader["evening"].ToString() + "$";
                    ret += myReader["night"].ToString() + "$";
                    ret += myReader["mo"].ToString() + "$";
                    ret += myReader["mi"].ToString() + "$";
                    ret += myReader["ev"].ToString() + "$";
                    ret += myReader["ni"].ToString() + "$";
                    ret += myReader["mid"].ToString() + "$";
                    ret += myReader["cmid"].ToString() + "$";
                    ret += myReader["crea"].ToString() + "$";
                    ret += "%";
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        // DC NEW ->

        internal string getMedicationConfirmations(string kid, DateTime dt)
        {
            string ret = "";
            string[] temp = kid.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night, cmc.morning mo,cmc.midday mi,cmc.evening ev,cmc.night ni, cmc.created crea, cm.cancelled cmca from clients c" +
                    " join clientsmedications cm on c.id = cm.client_id" +
                    " join medicaments m on cm.medicament_id = m.id" +
                    " join clientsmedicationsconfirmations cmc on cmc.clientsmedication_id = cm.id" +
                    " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" + temp[1].Replace(',', ' ') + "' " +
                    "and cm.to >= '" + dt.ToString("yyyy-MM-dd") + "' and cmc.for_day ='" + dt.ToString("yyyy-MM-dd") + "'", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    ret += myReader["mid"].ToString() + "$";
                    ret += myReader["name"].ToString() + "$";
                    ret += myReader["mo"].ToString() + "$";
                    ret += myReader["mi"].ToString() + "$";
                    ret += myReader["ev"].ToString() + "$";
                    ret += myReader["ni"].ToString() + "$";
                    ret += myReader["crea"].ToString() + "$";
                    ret += myReader["cmca"].ToString() + "$";
                    ret += "%";
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal bool medicationIsConfirmed(string kid, DateTime dt)
        {
            bool ret = false;
            string[] temp = kid.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select cmc.confirmed confi from clients c" +
                    " join clientsmedications cm on c.id = cm.client_id" +
                    " join medicaments m on cm.medicament_id = m.id" +
                    " join clientsmedicationsconfirmations cmc on cmc.clientsmedication_id = cm.id" +
                    " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" + temp[1].Replace(',', ' ') + "' " +
                    "and cm.to >= '" + dt.ToString("yyyy-MM-dd") + "' and cmc.for_day ='" + dt.ToString("yyyy-MM-dd") + "'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["confi"].ToString() == "1") {
                        ret = true;
                    }
                }
                myReader.Close();
                return ret;
            }
            catch 
            {
                MessageBox.Show("Die Medikation konnte nicht bestätigt werden", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void cancelMediForClient(string cmid)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("UPDATE clientsmedications SET cancelled = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' where id = " + cmid, _myConnection);
                myCommand.ExecuteNonQuery();
                _myConnection.Close();
            }
            catch
            {

            }
        }

        internal void deleteMediForClient(string cmid)
        {
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("DELETE FROM clientsmedications WHERE id = " + cmid, _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        //Select medicalactions.name a, clientsmedicalactions.realized b, clientsmedicalactions.statement c from clientsmedicalactions JOIN medicalactions ON medicalactions.id = clientsmedicalactions.medicalaction_id where client_id=" + rudi + " order by realized desc;", myConnection);
        internal void deleteMediActionForClient(string client, string date, string art, string desc)
        {
            string id = getMediActionIDbyName(art);
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("DELETE FROM clientsmedicalactions WHERE client_id = " + client + " and realized = '" + date + "' and statement = '" + desc + "' and medicalaction_id = " + id + ";" , _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        internal void addNewMedi(string name, string dis, string uid)
        {
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("INSERT INTO medicaments (created, modified, createuser_id, lastuser_id, name, description) " +
                    "values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" + uid + "', '" + uid + "', '" + name + "','" + dis + "')", _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        internal bool checkMediIfObsolete(string mid)
        {
            bool ret = false;
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = null;

                myCommand = new MySqlCommand("select `cancelled`, `to` from clientsmedications" +
                        " where `id` = '" + mid + "' AND ((`cancelled` > '0000-00-00' AND `cancelled` <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "') OR `to` < '" + DateTime.Now.ToString("yyyy-MM-dd") + "')", _myConnection);

                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    //MessageBox.Show(myReader["cancelled"].ToString());
                    /*if (myReader["cancelled"].ToString() != "01.01.0001 00:00:00" || Convert.ToDateTime(myReader["to"].ToString()) < DateTime.Today)
                    {
                        ret = true;
                    }*/
                    ret = true;
                }
                myReader.Close();
                return ret;
            }
            catch 
            {
                MessageBox.Show("Fehler bei der Überprüfung ob das Medikament noch aktuell ist! Bitte melden Sie dies dem Programmierer", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void updateMedi(string medi_id, string mid, string luid, string from, string to, string mo, string mi, string ev, string ni)
        {
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("UPDATE clientsmedications SET modified = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', lastuser_id = '" + luid + "', `from` = '" + from + "', `to` = '" + to + "', morning = '" + mo + "', midday = '" + mi + "', evening = '" + ev + "', night = '" + ni + "', medicament_id = '" + medi_id + "' where id = " + mid, _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        internal void addFunctions(string name)
        {
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("insert into functions (Name) values ('" + name + "')", _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        internal void editFunctions(string name, string id)
        {
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("update functions set name = '" + name + "' where id = " + id, _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        internal DateTime getMediDateFrom(string mid)
        {
            _myConnection.Open();
            DateTime dt = DateTime.Today;
            MySqlCommand myCommand = new MySqlCommand("select `from`, `to` from clientsmedications where `id` = '" + mid + "'", _myConnection);
            MySqlDataReader myReader = myCommand.ExecuteReader();
            

            while (myReader.Read())
            {
                dt = Convert.ToDateTime(myReader["from"].ToString());
            }
            myReader.Close();
            _myConnection.Close();
            return dt;
        }

        internal DateTime getMediDateTo(string mid)
        {
            _myConnection.Open();
            DateTime dt = DateTime.Today;
            MySqlCommand myCommand = new MySqlCommand("select `from`, `to` from clientsmedications where `id` = '" + mid + "'", _myConnection);
            MySqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                dt = Convert.ToDateTime(myReader["to"].ToString());
            }
            myReader.Close();
            _myConnection.Close();
            return dt;
        }


        internal void deluserfunc(int id)
        {
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("delete from userstofunctions where User = " + id, _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        internal void insertuserfunc(int id, UserFunctions fu)
        {
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("insert into userstofunctions values (" + id.ToString() + ", " + fu.id + ");", _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        internal void saveUserfunc(int id, List<UserFunctions> fu)
        {
            deluserfunc(id);
            foreach( UserFunctions u in fu)
            {
                insertuserfunc(id, u);
            }
        }

        internal List<UserFunctions> getUserFunctions()
        {
            List<UserFunctions> ret = new List<UserFunctions>();
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("select ID, Name from functions", _myConnection);
            MySqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                UserFunctions tmp = new UserFunctions();
                tmp.id = myReader["ID"].ToString();
                tmp.Name = myReader["Name"].ToString();
                ret.Add(tmp);
            }
            myReader.Close();
            _myConnection.Close();
            return ret;
        }

        internal List<UserFunctions> getUserFunctions(int id)
        {
            List<UserFunctions> ret = new List<UserFunctions>();
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("select functions.ID, functions.Name from functions join userstofunctions on functions.ID = userstofunctions.Function where userstofunctions.User = " + id.ToString(), _myConnection);
            MySqlDataReader myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                UserFunctions tmp = new UserFunctions();
                tmp.id = myReader["ID"].ToString();
                tmp.Name = myReader["Name"].ToString();
                ret.Add(tmp);
            }
            myReader.Close();
            _myConnection.Close();
            return ret;
        }

        internal string getMedicationForClient(string kid, DateTime dt, bool ignoreDate, bool confirmed)
        {
            string ret = "";
            string[] temp = kid.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = null;
                if (!confirmed)
                {
                    if (!ignoreDate)
                    {
                        myCommand = new MySqlCommand("select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night from clients c" +
                            " join clientsmedications cm on c.id = cm.client_id" +
                            " join medicaments m on cm.medicament_id = m.id" +
                            " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" + temp[1].Replace(',', ' ') + "' " +
                            "and cm.to >= '" + dt.ToString("yyyy-MM-dd") + "' and (cm.cancelled = '0000-00-00' or cm.cancelled >= '" + dt.ToString("yyyy-MM-dd") + "')", _myConnection);
                    }
                    else
                    {
                        myCommand = new MySqlCommand("select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night from clients c" +
                                                " join clientsmedications cm on c.id = cm.client_id" +
                                                " join medicaments m on cm.medicament_id = m.id" +
                                                " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" + temp[1].Replace(',', ' ') + "'", _myConnection);
                    }
                }
                else
                {
                    myCommand = new MySqlCommand("select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night, cmc.morning mo,cmc.midday mi,cmc.evening ev,cmc.night ni, cmc.created crea from clients c" +
                    " join clientsmedications cm on c.id = cm.client_id" +
                    " join medicaments m on cm.medicament_id = m.id" +
                    " join clientsmedicationsconfirmations cmc on cmc.clientsmedication_id = cm.id" +
                    " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" + temp[1].Replace(',', ' ') + "' " +
                    "and cm.to >= '" + dt.ToString("yyyy-MM-dd") + "' and (cm.cancelled = '0000-00-00' or cm.cancelled >= '" + dt.ToString("yyyy-MM-dd") + "') and cmc.for_day ='" + dt.ToString("yyyy-MM-dd") + "' and cmc.modified IS NULL", _myConnection); 
                }
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    ret += myReader["name"].ToString() + "$";
                    ret += myReader["morning"].ToString() + "$";
                    ret += myReader["midday"].ToString() + "$";
                    ret += myReader["evening"].ToString() + "$";
                    ret += myReader["night"].ToString() + "$";
                    ret += myReader["mid"].ToString() + "$";
                    ret += myReader["cmid"].ToString() + "$";
                    ret += "%";
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        // DC End

        internal string[] readDokuOverTime(string p, DateTime von, DateTime bis)
        {
            string[] ret = new string[5];

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                string[] temp = p.Split(' ');
                MySqlCommand myCommand = new MySqlCommand("select u.firstname first,u.lastname last, c.firstname,c.lastname,d.content_bodily, DATE_FORMAT(d.for_day,'%d.%m.%Y') AS forday, d.created, " +
                    "d.content_school, d.content_psychic, d.content_external_contact, d.content_responsibilities, d.createuser_id " +
                    "from clients c join clientsdailydocs d on c.id = d.client_id join users u on d.createuser_id=u.id where c.firstname ='"
                    + temp[0].Replace(',', ' ') + "' and c.lastname ='" + temp[1].Replace(',', ' ') + "' and d.for_day between'" + von.ToString("yyyy-MM-dd") + " 00:00' and '" + bis.ToString("yyyy-MM-dd") + " 23:59'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret[0] += "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_bodily"].ToString() + "\r\n\r\n";
                    ret[1] += "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_school"].ToString() + "\r\n\r\n";
                    ret[2] += "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_psychic"].ToString() + "\r\n\r\n";
                    ret[3] += "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_external_contact"].ToString() + "\r\n\r\n";
                    ret[4] += "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_responsibilities"].ToString() + "\r\n\r\n";
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;

            }
            finally
            {
                _myConnection.Close();
            }
        }


        internal string[] getDokuOverTime(string p, DateTime von, DateTime bis)
        {
            string[] ret = new string[1];

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                string[] temp = p.Split(' ');
                MySqlCommand myCommand = new MySqlCommand("select u.firstname first,u.lastname last, c.firstname,c.lastname,d.content_bodily, DATE_FORMAT(d.for_day,'%d.%m.%Y') AS forday, d.created, " +
                    "d.content_school, d.content_psychic, d.content_external_contact, d.content_responsibilities, d.createuser_id " +
                    "from clients c join clientsdailydocs d on c.id = d.client_id join users u on d.createuser_id=u.id where c.firstname ='"
                    + temp[0].Replace(',', ' ') + "' and c.lastname ='" + temp[1].Replace(',', ' ') + "' and d.for_day between'" + von.ToString("yyyy-MM-dd") + " 00:00' and '" + bis.ToString("yyyy-MM-dd") + " 23:59'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    //---------DC Begin---------
                    /*ret[0] += "Dokument: Körperlich " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_bodily"].ToString() + "$";
                    ret[0] += "Dokument: Schulisch " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_school"].ToString() + "$";
                    ret[0] += "Dokument: Psychisch " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_psychic"].ToString() + "$";
                    ret[0] += "Dokument: Außenkontakt " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_external_contact"].ToString() + "$";
                    ret[0] += "Dokument: Pflichten " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_responsibilities"].ToString() + "$";*/
                    if (myReader["content_bodily"].ToString() != "")
                    {
                        ret[0] += "Dokument: Körperlich " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_bodily"].ToString() + "$";
                    }
                    if (myReader["content_school"].ToString() != "")
                    {
                        ret[0] += "Dokument: Schulisch " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_school"].ToString() + "$";
                    }
                    if (myReader["content_psychic"].ToString() != "")
                    {
                        ret[0] += "Dokument: Psychisch " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_psychic"].ToString() + "$";
                    }
                    if (myReader["content_external_contact"].ToString() != "")
                    {
                        ret[0] += "Dokument: Außenkontakt " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_external_contact"].ToString() + "$";
                    }
                    if (myReader["content_responsibilities"].ToString() != "")
                    {
                        ret[0] += "Dokument: Pflichten " + "\r\n" + "Autor: " + myReader["first"].ToString() + " " + myReader["last"].ToString() + "\r\n" + " Klient: " + myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() + "\r\n Für den Tag: " + myReader["forday"].ToString() + "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_responsibilities"].ToString() + "$";
                    }
                    //---------DC End---------
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;

            }
            finally
            {
                _myConnection.Close();
            }
        }
        
        internal void setDoku(string kid, DateTime when, string koerperlich, string schulisch, string psychisch, string pflichten, string außen, string id)
        {
            try
            {
                string[] name = kid.Split(' ');
                _myConnection.Open();
                string clientId = "";

                MySqlCommand myCommand = new MySqlCommand("select id from clients where firstname ='"
                    + name[0].Replace(',', ' ') + "' and lastname ='" + name[1].Replace(',', ' ') + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    clientId = myReader["id"].ToString();
                }



                _myConnection.Close();
                _myConnection.Open();


                myCommand = new MySqlCommand("insert into clientsdailydocs (created, createuser_id, client_id, for_day,content_bodily, content_psychic, content_external_contact ,content_responsibilities ,content_school) " +
                    "values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "'," + id + ",'" + clientId + "','" + when.ToString("yyyy-MM-dd") + "','" + koerperlich.Replace("'", "`") + "','" + psychisch.Replace("'", "`") + "','" + außen.Replace("'", "`") + "','" + pflichten.Replace("'", "`") + "','" + schulisch.Replace("'", "`") + "')", _myConnection);
                int test = myCommand.ExecuteNonQuery();
                myReader.Close();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getNameByID(string id)
        {
            try
            {
                _myConnection.Open();
                string ret = "";

                MySqlCommand command = new MySqlCommand("select u.firstname, u.lastname from users u where u.id='" + id + "'", _myConnection);


                MySqlDataReader reader = command.ExecuteReader();
                string tmp = "";
                while (reader.Read())
                {
                    tmp = "";
                    tmp += reader["firstname"];
                    ret += tmp.Trim() + " ";
                    tmp = "";
                    tmp += reader["lastname"];
                    ret += tmp.Trim();



                }
                return ret;

            }
            catch (Exception e)
            {
                return e.ToString(); ;
            }
            finally
            {
                _myConnection.Close();
            }
        }
        public string getNameByIDLastFirst(string id)
        {
            try
            {
                _myConnection.Open();
                string ret = "";

                MySqlCommand command = new MySqlCommand("select u.firstname, u.lastname from users u where u.id='" + id + "'", _myConnection);


                MySqlDataReader reader = command.ExecuteReader();
                string tmp = "";
                while (reader.Read())
                {
                    tmp = "";
                    tmp += reader["lastname"];
                    ret += tmp.Trim() + " ";
                    tmp = "";
                    tmp += reader["firstname"];
                    ret += tmp.Trim();



                }
                return ret;

            }
            catch (Exception e)
            {
                return e.ToString(); ;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        #endregion

        /**************Ende (B)JB**************   (B) von Kostal :P */



        #region Global



        #region Get

        /// <summary>
        /// Getter for Database data 
        /// </summary>
        /// <param name="wg">all data you need from database</param>
        /// <returns>Db data</returns>
        /// 

        public string getWorkingTime(string p, DateTime dateTime)
        {
            string ret = "";

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                string[] temp = p.Split(' ');
                MySqlCommand myCommand = new MySqlCommand("select c.firstname,c.lastname, d.content_bodily, " +
                    "d.content_school, d.content_psychic, d.content_external_contact, d.content_responsibilities, d.createuser_id " +
                    "from clients c join clientsdailydocs d on c.id = d.client_id where c.firstname ='"
                    + temp[0].Replace(',', ' ') + "' and c.lastname ='" + temp[1].Replace(',', ' ') + "' and d.for_day between'" + dateTime.ToString("yyyy-MM-dd") + " 00:00' and '" + dateTime.ToString("yyyy-MM-dd") + " 23:59'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["content_bodily"].ToString() + "$";
                    ret += myReader["content_school"].ToString() + "$";
                    ret += myReader["content_psychic"].ToString() + "$";
                    ret += myReader["content_external_contact"].ToString() + "$";
                    ret += myReader["content_responsibilities"].ToString() + "$";
                    ret += myReader["createuser_id"].ToString() + "$";
                    ret += "%";
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getData(string database, string[] columnsDb, string[] columns, string conditions)
        {

            string ret = "";
            string allColumnsString = "";

            int i = 0;
            foreach (string column in columnsDb)
            {
                if (i < columnsDb.Length - 1)
                {
                    allColumnsString += column + ", ";

                }
                else
                {
                    allColumnsString += column;
                }
                i++;
            }

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                string selectCommand = "";
                string fullname = "";


                if (conditions != "")
                {
                    selectCommand = "select " + allColumnsString + " from " + database + " s " + conditions;
                }
                else
                {
                    selectCommand = "select " + allColumnsString + " from " + database + " s ";
                }

                MySqlCommand myCommand = new MySqlCommand(selectCommand, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    int colInt = 0;
                    foreach (string column in columns)
                    {
                        if (column == "firstname" || column == "lastname")
                        {
                            fullname += myReader[column].ToString() + " ";
                            if (column == "lastname")
                            {
                                ret += fullname.Replace("%", "!!!PROZENTZEICHEN!!!");
                                if (colInt < columns.Length - 1)
                                {
                                    ret += "$";
                                }
                                fullname = "";
                            }
                        }
                        else
                        {
                            ret += myReader[column].ToString().Replace("%", "!!!PROZENTZEICHEN!!!");
                            if (colInt < columns.Length - 1)
                            {
                                ret += "$";
                            }
                        }
                        colInt++;
                    }

                    ret += "%";



                }
                ret = ret.Substring(0, ret.Length - 1);
                //ret.TrimEnd('%');
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        #endregion

        #region Set

        /// <summary>
        /// Setter for Database data
        /// </summary>
        /// <param name="wg">set methode automatic fill created , created_id, modified and lastuser_id</param>
        /// <returns>Db set</returns>
        public void setData(string database, List<string> dataColumns, List<string> data)
        {
            List<string> columns = new List<string> { "created", "createuser_id", "modified", "lastuser_id" };
            List<string> datas = new List<string> { DateTime.Now.ToString("yyyy-MM-dd HH:mm"), Functions.Functions.ActualUserFromList.Id.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm"), Functions.Functions.ActualUserFromList.Id.ToString() };
            string allColumnsString = "";
            string allData = "";

            columns.AddRange(dataColumns);
            datas.AddRange(data);

            int i = 0;
            foreach (string column in columns)
            {
                if (i < columns.Count - 1)
                {
                    allColumnsString += column + ", ";
                }
                else
                {
                    allColumnsString += column;
                }
                i++;
            }

            i = 0;
            foreach (string dat in datas)
            {
                if (i < datas.Count - 1)
                {
                    allData += "'" + dat + "', ";
                }
                else
                {
                    allData += "'" + dat + "'";
                }
                i++;
            }
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("insert into " + database + " ( " + allColumnsString + " ) values( " + allData + " )", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        /// <summary>
        /// Setter for Database data
        /// </summary>
        /// <param name="wg">Full set only created and modified datetime automatic fill</param>
        /// <returns>Db set</returns>
        public void setFullData(string database, List<string> dataColumns, List<string> data)
        {
            List<string> columns = new List<string> { "created", "modified" };
            string allColumnsString = "";
            string allData = "";

            columns.AddRange(dataColumns);

            int i = 0;
            foreach (string column in columns)
            {
                if (i < columns.Count - 1)
                {
                    allColumnsString += column + ", ";
                }
                else
                {
                    allColumnsString += column;
                }
                i++;
            }

            i = 0;
            foreach (string dat in data)
            {
                if (i < data.Count - 1)
                {
                    allData += "'" + dat + "', ";
                }
                else
                {
                    allData += "'" + dat + "'";
                }
                i++;
            }
            try
            {
                _myConnection.Open();
                //MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("insert into " + database + " (" + allColumnsString + ") values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' , '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' ," + allData + ")", _myConnection);
                myCommand.ExecuteNonQuery();
                //myReader.Close();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Edit/update for Database data
        /// </summary>
        /// <param name="wg">update column and data to database</param>
        /// <returns>Db update</returns>
        public void updateData(string database, List<string> dataColumns, List<string> data, int id)
        {
            List<string> columns = new List<string> { "modified", "lastuser_id" };
            List<string> dataList = new List<string>() { DateTime.Now.ToString("yyyy-MM-dd HH:mm"), Functions.Functions.ActualUserFromList.Id.ToString() };
            string updateString = "";

            columns.AddRange(dataColumns);
            dataList.AddRange(data);

            int i = 0;
            foreach (string column in columns)
            {
                if (i < columns.Count - 1)
                {
                    updateString = updateString + column + "='" + dataList[i] + "', ";
                }
                else
                {
                    updateString = updateString + column + "='" + dataList[i] + "'";
                }
                i++;
            }

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("UPDATE " + database + " SET " + updateString + " where id = " + id, _myConnection);
                myCommand.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }


        #region Delete
        /// <summary>
        /// Delete for Database data
        /// </summary>
        /// <param name="wg">set data inactive in db</param>
        /// <returns>inactive Db data</returns>
        //write delete function here - keine Spalte für inaktiv/aktiv in DB

        #endregion

        #endregion

        #endregion

        #region Employee

        public string getEmployees()
        {
            return getData("users", new string[] { "s.id", "s.firstname", "s.lastname", "s.username", "s.inclusion" }, new string[] { "id", "firstname", "lastname", "username", "inclusion" }, " where s.leaving IS NULL and s.inclusion is not null order by s.lastname, s.firstname");
        }

        public void setEmployee(List<string> data)
        {
            List<string> columns = new List<string> { "firstname", "lastname", "username", "inclusion" };
            setData("users", columns, data);
        }

        public void updateEmployee(List<string> data, int id)
        {
            updateData("users", new List<string> { "firstname", "lastname", "username", "inclusion" }, data, id);
        }

        #endregion

        #region Extern

        public string getExtern()
        {
            return getData("users", new string[] { "firstname", "lastname", "username", "inclusion" }, new string[] { "firstname", "lastname", "username", "inclusion" }, " where s.leaving IS NULL and s.inclusion is null order by s.lastname, s.firstname");
        }

        #endregion

        #region Holiday Requests

        public string getHolidayRequests()
        {
            return getData("usersholidays", new string[] { "u.firstname", "u.lastname", "s.date_from", "s.date_to", "s.admin_status" }, new string[] { "createuser_id", "date_from", "date_to", "admin_status" }, " order by s.date_from, s.date_to");
        }

        #endregion

        #region Clients

        public string getClients()
        {
            return getData("clients", new string[] { "s.firstname", "s.lastname", "s.inclusion", "s.date_of_birth", "s.status", "s.mc_phone_1" }, new string[] { "firstname", "lastname", "inclusion", "date_of_birth", "status", "mc_phone_1" }, " where s.leaving IS NULL and s.status = 0  order by s.lastname, s.firstname");
        }

        #endregion

        #region Medicaments

        public string getMedicaments()
        {
            return getData("medicaments", new string[] { "s.id", "s.name" }, new string[] { "id", "name" }, " order by s.name ");
        }

        public void setMedicament(List<string> data)
        {
            setData("medicaments", new List<string> { "name", "createuser_id", "lastuser_id", "description" }, data);
        }

        public void updateMedicaments(List<string> data, int id)
        {
            updateData("medicaments", new List<string> { "name", "createuser_id", "lastuser_id", "description" }, data, id);
        }

        #region Medicaments by Client

        public string getMedicamentsByClient(int id)
        {
            return getData("clientsmedications", new string[] { "s.medicament_id", "s.from", "s.to", "s.morning", "s.midday", "s.evening", "s.night" }, new string[] { "medicament_id", "from", "to", "morning", "midday", "evening", "night" }, " where s.client_id = " + id);
        }

        public void setMedicamentByClient(List<string> data)
        {
            List<string> columns = new List<string> { "name", "createuser_id", "lastuser_id", "end" };
            setData("clientsmedications", columns, data);
        }

        public void addMedicamentForClient(string[] data)
        {
            _myConnection.Open();
            MySqlCommand myCommand = new MySqlCommand("INSERT INTO clientsmedications (`client_id`, `created`, `modified`, `createuser_id`, `lastuser_id`, `medicament_id`, `from`, `to`, `morning`, `midday`, `evening`, `night`) VALUES ('" + data[0] + "', '" + data[1] + "', '" + data[2] + "', '" + data[3] + "', '" + data[4] + "', '" + data[5] + "', '" + data[6] + "', '" + data[7] + "', '" + data[8] + "', '" + data[9] + "', '" + data[10] + "', '" + data[11] + "');", _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        public void updateMedicamentByClient(List<string> data, int id)
        {
            List<string> columns = new List<string> { "medicament_id", "from", "to", "morning", "midday", "evening", "night" };
            updateData("clientsmedications", columns, data, id);
        }

        #endregion

        #endregion

        #region Medical Action

        #endregion

        #region Hour Types / Projects

        public string getProjects()
        {
            return getData("projects", new string[] { "s.id", "s.name" }, new string[] { "id", "name" }, " order by s.name");
        }

        public void setHourTypes(List<string> data)
        {
            List<string> columns = new List<string> { "name" };
            setData("projects", columns, data);
        }

        public void updateHourTypes(List<string> data, int id)
        {
            List<string> columns = new List<string> { "name" };
            updateData("projects", columns, data, id);
        }

        #endregion

        #region Services

        public string getServices()
        {
            return getData("services", new string[] { "s.id", "s.name" }, new string[] { "id", "name" }, " order by s.name");
        }

        #endregion

        #region User

        public string getUserID(string name)
        {
            string ret = "";
            string[] names;
            try
            {
                names = name.Split(' ');
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select u.id from users u where u.username='" + name + "'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["id"].ToString();
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getUserIDbyFullname(string name)
        {
            string ret = "";
            string[] fullname = name.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select u.id from users u where u.firstname='" + fullname[0] + "' AND u.lastname='" + fullname[1] + "'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["id"].ToString();
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<User> getUsers()
        {
            List<User> us5 = new List<User>();

            try
            {

                _myConnection.Open();
                MySqlDataReader reader = null;
                MySqlCommand myCommand = new MySqlCommand("select id from users", _myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    User us = new User();
                    us.Id = reader["id"].ToString();


                    //firstname, lastname, isAdmin und services 
                    us5.Add(us);
                }
                return us5;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<User> getUsers_working()
        {
            List<User> us5 = new List<User>();

            try
            {

                _myConnection.Open();
                MySqlDataReader reader = null;
                MySqlCommand myCommand = new MySqlCommand("select id from users where leaving is NULL order by lastname", _myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    User us = new User();
                    us.Id = reader["id"].ToString();


                    //firstname, lastname, isAdmin und services 
                    us5.Add(us);
                }
                return us5;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        #endregion

        #region Working Time


        public string getUrlaubstime(int user)
        {
            /*CREATE TABLE workingtime(
	        tid int(10) auto_increment primary key,
	        usersid int(10) references Users.id,
	        art varchar(50),
	        datetimefrom datetime,
	        datetimeto datetime,
	        comment varchar(100)
            );
             */
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("select users.holidays_open FROM users WHERE users.id = " + user, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["holidays_open"].ToString();
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public string delWorkingtime(int user, DateTime from, DateTime to, string comment)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("DELETE FROM workingtime WHERE usersid='" + user + "' AND DAY(datetimefrom)=" + from.Day + " AND MONTH(datetimefrom)=" + from.Month + " AND YEAR(datetimefrom)=" + from.Year + " AND DAY(datetimeto)=" + to.Day + " AND MONTH(datetimeto)=" + to.Month + " AND YEAR(datetimeto)=" + to.Year + " AND comment ='" + comment + "' AND MINUTE(datetimefrom)=" + from.Minute + " AND HOUR(datetimefrom)=" + from.Hour + " AND MINUTE(datetimeto)=" + to.Minute + " AND HOUR(datetimeto)=" + to.Hour, _myConnection);
                myCommand.ExecuteNonQuery();
                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public string getUberstdges(int uid)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select overtime from users where id=" + uid + ";", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret = myReader["overtime"].ToString();
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }


        public string getSonnUFeier(int id, DateTime dat)
        {
            /*CREATE TABLE workingtime(
	        tid int(10) auto_increment primary key,
	        usersid int(10) references Users.id,
	        art varchar(50),
	        datetimefrom datetime,
	        datetimeto datetime,
	        comment varchar(100)
            );
             */
            
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select art, datetimefrom, datetimeto from workingtime where usersid=" + id.ToString() + " AND (DAYOFWEEK(datetimefrom) = 7 OR DATE(datetimefrom) in (select date_holiday from officialholidays)) AND YEAR(datetimefrom) = " + dat.Year + " AND MONTH(datetimefrom) = " + dat.Month + " AND comment not like 'Krank%';", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["art"].ToString() + "$";
                    ret += myReader["datetimefrom"].ToString() + "$";
                    ret += myReader["datetimeto"].ToString();
                    ret += "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public string getWorkingtime(DateTime date, int user, bool isadmin)
        {
            /*CREATE TABLE workingtime(
	        tid int(10) auto_increment primary key,
	        usersid int(10) references Users.id,
	        art varchar(50),
	        datetimefrom datetime,
	        datetimeto datetime,
	        comment varchar(100)
            );
             */
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand;
                if (user == 0)
                {
                    if (isadmin)
                    {
                        myCommand = new MySqlCommand("select users.firstname, users.lastname, workingtime.art, workingtime.datetimefrom, workingtime.datetimeto, workingtime.comment, workingtime.holidayverfied FROM workingtime JOIN users ON workingtime.usersid = users.id WHERE (usersid = usersid OR (workingtime.holidayverfied = false)) AND (MONTH(datetimefrom) = " + date.Month + " OR (holidayverfied = false AND art = 'Urlaub')) AND (YEAR(datetimefrom) = " + date.Year + " OR (holidayverfied = false AND art = 'Urlaub'));", _myConnection);
                    }
                    else
                    {
                        myCommand = new MySqlCommand("select users.firstname, users.lastname, workingtime.art, workingtime.datetimefrom, workingtime.datetimeto, workingtime.comment, workingtime.holidayverfied FROM workingtime JOIN users ON workingtime.usersid = users.id WHERE (usersid = usersid) AND (MONTH(datetimefrom) = " + date.Month + " OR (holidayverfied = false AND art = 'Urlaub')) AND (YEAR(datetimefrom) = " + date.Year + " OR (holidayverfied = false AND art = 'Urlaub'));", _myConnection);
                    }
                }
                else
                {
                    if (isadmin)
                    {
                        myCommand = new MySqlCommand("select users.firstname, users.lastname, workingtime.art, workingtime.datetimefrom, workingtime.datetimeto, workingtime.comment, workingtime.holidayverfied FROM workingtime JOIN users ON workingtime.usersid = users.id WHERE (usersid = " + user + " OR (holidayverfied = false AND art = 'Urlaub')) AND (MONTH(datetimefrom) = " + date.Month + " OR (holidayverfied = false AND art = 'Urlaub')) AND (YEAR(datetimefrom) = " + date.Year + " OR (holidayverfied = false AND art = 'Urlaub'));", _myConnection);
                    }
                    else
                    {
                        myCommand = new MySqlCommand("select users.firstname, users.lastname, workingtime.art, workingtime.datetimefrom, workingtime.datetimeto, workingtime.comment, workingtime.holidayverfied FROM workingtime JOIN users ON workingtime.usersid = users.id WHERE (usersid = " + user + ") AND (MONTH(datetimefrom) = " + date.Month + " OR (holidayverfied = false AND art = 'Urlaub')) AND (YEAR(datetimefrom) = " + date.Year + " OR (holidayverfied = false AND art = 'Urlaub'));", _myConnection);
                    }
                }
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["firstname"].ToString() + "$";
                    ret += myReader["lastname"].ToString() + "$";
                    ret += myReader["art"].ToString() + "$";
                    ret += myReader["datetimefrom"].ToString() + "$";
                    ret += myReader["datetimeto"].ToString() + "$";
                    ret += myReader["comment"].ToString() + "$";
                    ret += myReader["holidayverfied"].ToString();
                    ret += "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public double getWorkinghoursperWeek(string id)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select users.weeklyhours FROM users WHERE users.id = " + id, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["weeklyhours"].ToString();
                }

                myReader.Close();

                return Convert.ToDouble(ret);
            }
            catch 
            {
                MessageBox.Show("Es is ein Fehler aufgetreten! Ich weiß leider nicht warum :(", ":(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _myConnection.Close();
            }
            return 0;
        }

        public string getNonWorkingDays()
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select date_holiday FROM officialholidays", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["date_holiday"].ToString() + "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getWorkingtimeUsers()
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select username, firstname, lastname, id FROM users WHERE leaving is NULL order by lastname, firstname", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["username"].ToString() + "$";
                    ret += myReader["firstname"].ToString() + "$";
                    ret += myReader["lastname"].ToString() + "$";
                    ret += myReader["id"].ToString() + "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public bool setHolidaytime(int uid, string art, DateTime datetimefrom, DateTime datetimeto, string comment)
        {
            int test = 0;
            if (exists(uid, art, datetimefrom, datetimeto, comment) == false)
            {
                try
                {
                    _myConnection.Open();
                    MySqlCommand myCommand = new MySqlCommand("insert into workingtime (usersid, art, datetimefrom, datetimeto, comment, holidayverfied) values (" + uid.ToString() + ", '" + art + "', '" + datetimefrom.Year + "." + datetimefrom.Month + "." + datetimefrom.Day + " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00" + "', '" + datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " + datetimeto.Hour + ":" + datetimeto.Minute + ":00" + "', '" + comment + "', 0);", _myConnection);
                    test = myCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    _myConnection.Close();
                }
            }
            if (test == 0)
                return false;
            else
                return true;
        }

        public bool setHolidayverfied(int uid, DateTime datetimefrom, DateTime datetimeto, int isverifed, int adminid)
        {
            int test = 0;
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;
                if (isverifed == -1)
                {
                    myCommand = new MySqlCommand("UPDATE workingtime SET holidayverfied= NULL, von= " + adminid + " WHERE usersid=" + uid + " AND datetimefrom= '" + datetimefrom.Year + "." + datetimefrom.Month + "." + datetimefrom.Day + " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00' AND datetimeto= '" + datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " + datetimeto.Hour + ":" + datetimeto.Minute + ":00" + "'", _myConnection);
                }
                else
                {
                    myCommand = new MySqlCommand("UPDATE workingtime SET holidayverfied=" + isverifed.ToString() + " , von= " + adminid + " WHERE usersid=" + uid + " AND datetimefrom= '" + datetimefrom.Year + "." + datetimefrom.Month + "." + datetimefrom.Day + " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00' AND datetimeto= '" + datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " + datetimeto.Hour + ":" + datetimeto.Minute + ":00" + "'", _myConnection);
                }

                test = myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                _myConnection.Close();
            }
            if (test == 0)
                return false;
            else
                return true;
        }

        public bool exists(int uid, string art, DateTime datetimefrom, DateTime datetimeto, string comment)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select COUNT(*) as 'anz' from workingtime where usersid = " + uid.ToString() + " AND art = '" + art + "' AND  datetimefrom = '" + datetimefrom.Year + "." + datetimefrom.Month + "." + datetimefrom.Day + " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00" + "' AND datetimeto = '" + datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " + datetimeto.Hour + ":" + datetimeto.Minute + ":00" + "' AND comment = '" + comment + "' AND holidayverfied is not NULL;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["anz"].ToString();
                }
                _myConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                _myConnection.Close();
            }
            if (ret == "0")
                return false;
            else
                return true;
        }

        public bool setWorkingtime(int uid, string art, DateTime datetimefrom, DateTime datetimeto, string comment)
        {
            int test = 0;
            if (exists(uid, art, datetimefrom, datetimeto, comment) == false)
            {
                try
                {
                    _myConnection.Open();
                    MySqlCommand myCommand = new MySqlCommand("insert into workingtime (usersid, art, datetimefrom, datetimeto, comment) values (" + uid.ToString() + ", '" + art + "', '" + datetimefrom.Year + "." + datetimefrom.Month + "." + datetimefrom.Day + " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00" + "', '" + datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " + datetimeto.Hour + ":" + datetimeto.Minute + ":00" + "', '" + comment + "');", _myConnection);
                    test = myCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    _myConnection.Close();
                }
            }
            if (test == 0)
                return false;
            else
                return true;
        }

        public string getWorkingTimeByUser(string id)
        {
            //return getData("usersworkingtimes", new string[] { "s.datetime_from", "s.datetime_to", "s.comment", "p.name", "s.type" }, new string[] { "datetime_from", "datetime_to", "comment", "name", "type" }, " join projects p on p.id = s.project where s.createuser_id = " + id);

            return "";
        }

        public string getWorkingTimeById(string userId, string id)
        {
            return "";
            //return getData("usersworkingtimes", new string[] { "s.datetime_from", "s.datetime_to", "s.comment", "s.project", "s.type" }, new string[] { "datetime_from", "datetime_to", "comment", "project", "type" }, " where s.createuser_id = " + userId + " and s.id = " + id);
        }

        public void setWorkingTime(List<string> data)
        {
            setData("usersworkingtimes", new List<string> { "s.datetime_from", "s.datetime_to", "s.comment", "s.project", "s.type" }, data);
        }

        public void updateWorkingTime(List<string> data, int id)
        {
            updateData("usersworkingtimes", new List<string> { "s.datetime_from", "s.datetime_to", "s.comment", "s.project", "s.type" }, data, id);
        }

        #endregion

        #region Instructions

        public void setInstruction(String date, String title, String desc, String uid) // readinstructions gelesene instructions
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("INSERT INTO workinginstructions(title, describtion , startdate , uid) VALUES ('" + title + "' , '" + desc + "' , '" + date + "'" + " , " + uid + ");", _myConnection);
                String instructionid = "";
                myCommand.ExecuteNonQuery();
                myCommand = new MySqlCommand("select max(iid) abc from workinginstructions", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    instructionid = myReader["abc"].ToString();
                }
                myReader.Close();
                myCommand = new MySqlCommand("insert into readinstructions(iid,pid,checked) select " + instructionid + " , id , 0 from users", _myConnection);
                myCommand.ExecuteNonQuery();
                myCommand = new MySqlCommand("update readinstructions set checked=1 where pid=" + uid + " and iid=" + instructionid, _myConnection);
                myCommand.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        public List<Instruction> getInstruction()
        {
            try
            {
                List<Instruction> list = new List<Instruction>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select startdate,title,describtion, uid from workinginstructions", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String startdate = myReader["startdate"].ToString();
                    String title = myReader["title"].ToString();
                    String desc = myReader["describtion"].ToString();
                    String uid = myReader["uid"].ToString();
                    list.Add(new Instruction(title, desc, startdate, uid, this));
                }
                myReader.Close();

                return list;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }


        public void setInstructionRead(String p, String titel, String date, String desc)
        {
            try
            {

                _myConnection.Open();
                String instructionid = "";
                MySqlCommand myCommand = new MySqlCommand("select iid abc from workinginstructions where title='" + titel + "' and startdate='" + date + "' and describtion='" + desc + "';", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    instructionid = myReader["abc"].ToString();
                }
                myReader.Close();
                myCommand = new MySqlCommand("update readinstructions set checked=1 where pid=" + p + " and iid=" + instructionid, _myConnection);
                myCommand.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<ReadInstruction> getInstructionRead(String titel, String date, String desc)
        {
            try
            {

                _myConnection.Open();
                List<ReadInstruction> list = new List<ReadInstruction>();
                String instructionid = "";
                MySqlCommand myCommand = new MySqlCommand("select iid abc from workinginstructions where title='" + titel + "' and startdate='" + date + "' and describtion='" + desc + "';", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    instructionid = myReader["abc"].ToString();
                }
                myReader.Close();
                myCommand = new MySqlCommand("select  u.firstname f, u.lastname l , ri.checked c from readinstructions ri join users u on ri.pid = u.id where iid=" + instructionid + ";", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String firstname = myReader["f"].ToString();
                    String lastname = myReader["l"].ToString();
                    String check = myReader["c"].ToString();
                    list.Add(new ReadInstruction(firstname, lastname, check));
                }
                myReader.Close();
                return list;

            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<Instruction> getUnreadInstruction(String id)
        {
            try
            {
                List<Instruction> list = new List<Instruction>();
                _myConnection.Open();
                List<String> sl = new List<string>();
                MySqlCommand myCommand = new MySqlCommand("select iid abc from readinstructions where pid=" + id + " and checked = 0", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String instructionid = myReader["abc"].ToString();
                    sl.Add(instructionid);
                }
                myReader.Close();
                foreach (String laufuid in sl)
                {
                    myCommand = new MySqlCommand("Select startdate,title,describtion, uid from workinginstructions where iid=" + laufuid + ";", _myConnection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        String startdate = myReader["startdate"].ToString();
                        String title = myReader["title"].ToString();
                        String desc = myReader["describtion"].ToString();
                        String uid = myReader["uid"].ToString();
                        list.Add(new Instruction(title, desc, startdate, uid, this));
                    }
                    myReader.Close();
                }

                return list;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }



        #endregion

        #region tasks new

        public void setTasks(String uid1, String uid2, String startdate, String enddate, String desc)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("insert into taskt (uid1, uid2, startdate, enddate, descr, status) values (" + uid1 + " , " + uid2 + " , '" + startdate + "' , '" + enddate + "' , '" + desc + "' , " + 0 + ");", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void changeModefrom0to1(String uid1, String uid2, String startdate, String enddate, String desc)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("update taskt set status=1 where uid1=" + uid1 + " and uid2=" + uid2 + " and startdate='" + startdate + "' and enddate='" + enddate + "' and descr='" + desc + "';", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void changeModefrom1to2(String uid1, String uid2, String startdate, String enddate, String desc)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("update taskt set status=2 where uid1=" + uid1 + " and uid2=" + uid2 + " and startdate='" + startdate + "' and enddate='" + enddate + "' and descr='" + desc + "';", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void changeEnddateTasks(String uid1, String uid2, String startdate, String enddate, String desc)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("update taskt set enddate='" + enddate + "'where uid1=" + uid1 + " and uid2=" + uid2 + " and startdate='" + startdate + "' and descr='" + desc + "';", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void changeModefrom1to0(String uid1, String uid2, String startdate, String enddate, String desc)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("update taskt set status=0 where uid1=" + uid1 + " and uid2=" + uid2 + " and startdate='" + startdate + "' and enddate='" + enddate + "' and descr='" + desc + "';", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }


        public List<Task> getTasksfromUser(String id)
        {
            try
            {
                _myConnection.Open();
                List<Task> list = new List<Task>();
                MySqlCommand myCommand = new MySqlCommand("select uid1, uid2, startdate, enddate, descr from taskt where uid2=" + id + " and status=0;", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String uid1 = myReader["uid1"].ToString();
                    String uid2 = myReader["uid2"].ToString();
                    String startdate = myReader["startdate"].ToString();
                    String enddate = myReader["enddate"].ToString();
                    String desc = myReader["descr"].ToString();
                    list.Add(new Task(uid1, uid2, startdate, enddate, desc, this));

                }
                myReader.Close();

                return list;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<Task> getTasksforUser(String id)
        {
            try
            {
                _myConnection.Open();
                List<Task> list = new List<Task>();
                MySqlCommand myCommand = new MySqlCommand("select uid1, uid2, startdate, enddate, descr from taskt where uid1=" + id + " and status=1;", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String uid1 = myReader["uid1"].ToString();
                    String uid2 = myReader["uid2"].ToString();
                    String startdate = myReader["startdate"].ToString();
                    String enddate = myReader["enddate"].ToString();
                    String desc = myReader["descr"].ToString();
                    list.Add(new Task(uid1, uid2, startdate, enddate, desc, this));

                }
                myReader.Close();

                return list;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<Task> getCreatedTasksforUser(String id)
        {
            try
            {
                _myConnection.Open();
                List<Task> list = new List<Task>();
                MySqlCommand myCommand = new MySqlCommand("select uid1, uid2, startdate, enddate, descr from taskt where uid1=" + id + " and status=0;", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String uid1 = myReader["uid1"].ToString();
                    String uid2 = myReader["uid2"].ToString();
                    String startdate = myReader["startdate"].ToString();
                    String enddate = myReader["enddate"].ToString();
                    String desc = myReader["descr"].ToString();
                    list.Add(new Task(uid1, uid2, startdate, enddate, desc, this));

                }
                myReader.Close();

                return list;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }


        public List<Task> getUrgentTasksfromUser(String id)
        {
            try
            {
                _myConnection.Open();
                List<Task> list = new List<Task>();
                MySqlCommand myCommand = new MySqlCommand("select uid1, uid2, startdate, enddate, descr from taskt where uid2=" + id + " and (Datediff(enddate,CurDate())<=5  or CurDate() >= enddate)  and status=0;", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String uid1 = myReader["uid1"].ToString();
                    String uid2 = myReader["uid2"].ToString();
                    String startdate = myReader["startdate"].ToString();
                    String enddate = myReader["enddate"].ToString();
                    String desc = myReader["descr"].ToString();
                    list.Add(new Task(uid1, uid2, startdate, enddate, desc, this));

                }
                myReader.Close();

                return list;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }



        #endregion

        #region Tasks

        public string getTasksFromUser(string id)
        {
            return getData("tasks", new string[] { "s.id, u.firstname", "u.lastname", "s.name", "s.end", "s.status" }, new string[] { "id", "firstname", "lastname", "name", "end", "status" }, " join  users u on u.id = s.createuser_id where s.user_id = " + id + " and s.end is not null and s.end > NOW() order by s.end ASC");
        }

        public string getImportantTasksFromUser(string id)
        {
            return getData("tasks", new string[] { "s.id, u.firstname", "u.lastname", "s.name", "s.end", "s.status" }, new string[] { "id", "firstname", "lastname", "name", "end", "status" }, " join  users u on u.id = s.user_id where s.user_id = " + id + " and s.end between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00' and '" + DateTime.Now.AddDays(5).ToString("yyyy-MM-dd") + " 23:59' order by s.end, u.firstname DESC");
        }
        public string getTasksForUserById(string id)
        {
            return getData("tasks", new string[] { "s.id, u.firstname", "u.lastname", "s.name", "s.description", "s.project_id", "s.start", "s.end", "s.lastuser_id", "s.status", "s.createuser_id" }, new string[] { "id", "firstname", "lastname", "name", "description", "project_id", "start", "end", "lastuser_id", "status", "createuser_id" }, "join users u on s.lastuser_id = u.id where s.id = " + id + " order by s.end, u.firstname");
        }

        public string getTasksForUser(string id)
        {
            return getData("tasks", new string[] { "s.id, u.firstname", "u.lastname", "name", "s.end", "s.status" }, new string[] { "id", "firstname", "lastname", "name", "end", "status" }, " join  users u on s.createuser_id = u.id where s.user_id = " + id + " and s.end is not null and s.end > NOW() order by s.end DESC");
        }

        public string getTasksFromUserById(string id)
        {
            return getData("tasks", new string[] { "s.id, u.firstname", "u.lastname", "s.name", "s.description", "s.project_id", "s.start", "s.end", "s.user_id", "status", "s.createuser_id" }, new string[] { "id", "firstname", "lastname", "name", "description", "project_id", "start", "end", "user_id", "status", "createuser_id" }, "join users u on s.createuser_id = u.id where s.id = " + id + " order by s.end, u.firstname");
        }

        public void setTask(List<string> data)
        {
            setData("tasks", new List<string> { "user_id", "start", "end", "name", "description", "project_id", "status" }, data);
        }

        public void updateTask(List<string> data, int id)
        {
            updateData("tasks", new List<string> { "user_id", "start", "end", "name", "description", "project_id", "status" }, data, id);
        }
        #endregion

        #region vital

        public List<String> getVital(String id)
        {
            try
            {
                List<String> values = new List<String>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select sex, firstname, lastname, inclusion, date_of_birth, citizenship, district_authority, insurance, leaving, icd, place_of_birth, social_insurance_number, co_insured, contact_id, status, assignment from clients where id=" + id + ";", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    values.Add(myReader["sex"].ToString());
                    values.Add(myReader["firstname"].ToString());
                    values.Add(myReader["lastname"].ToString());
                    values.Add(myReader["inclusion"].ToString());
                    values.Add(myReader["date_of_birth"].ToString());
                    values.Add(myReader["citizenship"].ToString());
                    values.Add(myReader["district_authority"].ToString());
                    values.Add(myReader["insurance"].ToString());
                    values.Add(myReader["leaving"].ToString());
                    values.Add(myReader["icd"].ToString());
                    values.Add(myReader["place_of_birth"].ToString());
                    values.Add(myReader["social_insurance_number"].ToString());
                    values.Add(myReader["co_insured"].ToString());
                    values.Add(myReader["contact_id"].ToString());
                    values.Add(myReader["status"].ToString());
                    values.Add(myReader["assignment"].ToString());
                }
                myReader.Close();
                return values;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }


        #region Contacts

        public void setContacts(String createuserid, String institution, String salutation_id, String title_id, String firstname, String lastname, String job_company, String job_department, String job_street, String job_zip, String job_country, String job_city, String job_phone_1, String job_email, String comment, String job_function)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("insert into contacts (created, createuser_id, institution, salutation_id, title_id, firstname, lastname, job_company, job_department, job_street, job_zip, job_country, job_city, job_phone_1, job_email, comment, job_function) values ('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' , " + createuserid.Trim() + " , '" + institution.Trim() + "' , " + salutation_id + " , " + title_id +" , '" + firstname.Trim() + "' , '" + lastname.Trim() + "' , '" + job_company.Trim() + "' , '" + job_department.Trim() + "' , '" + job_street.Trim() + "' , '" + job_zip.Trim() + "' , '" + job_country.Trim() + "' , '" + job_city.Trim() + "' , '" + job_phone_1.Trim() + "' , '" + job_email.Trim() + "' , '" + comment.Trim() + "' , '" + job_function.Trim() + "');", _myConnection);
                //myCommand = new MySqlCommand("insert into contacts (created, createuser_id, institution, salutation_id, title_id, firstname, lastname, job_company, job_department, job_street, job_zip, job_country, job_city, job_phone_1, job_phone_2, job_fax, job_email, job_www, job_skype, comment, groups, job_function) values ('" + DateTime.Now.ToString() + "' , '" + id.Trim() + "' , '" + s1.Trim() + "' , (select id from salutations where name='" + s2.Trim() + "') , (select id from titles where name='" + s3.Trim() + "') , '" + s4.Trim() + "' , '" + s5.Trim() + "' , '" + s6.Trim() + "' , '" + s7.Trim() + "' , '" + s8.Trim() + "' , '" + s9.Trim() + "' , '" + s10.Trim() + "' , '" + s11.Trim() + "' , '" + s12.Trim() + "' , '" + s13.Trim() + "' , '" + s14.Trim() + "' , '" + s15.Trim() + "' , '" + s16.Trim() + "' , '" + s17.Trim() + "' , '" + s18.Trim() + "' , (select id from groups where name='" + s19.Trim() + "') , '" + s20.Trim() + "');", myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch 
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten!\nBitte überprüfen Sie die Eingabe!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<Salutations> getsalutations()
        {
            List<Salutations> ret = new List<Salutations>();
            try
            {

                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select id, name, name_long from salutations;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret.Add(new Salutations(myReader["id"].ToString(), myReader["name"].ToString(), myReader["name_long"].ToString()));
                }
                myReader.Close();
                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void changeContacts(String cid, String id, String s1, String s2, String s3, String s4, String s5, String s6, String s7, String s8, String s9, String s10, String s11, String s12, String s13, String s14, String s15, String s16, String s17, String s18, String s19, String s20)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("update contacts set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', lastuser_id = '" + id.Trim() + "', institution = '" + s1.Trim() + "', salutation_id = (select id from salutations where name='" + s2.Trim() + "'), title_id = (select id from titles where name='" + s3.Trim() + "'), firstname = '" + s4.Trim() + "', lastname = '" + s5.Trim() + "', job_company = '" + s6.Trim() + "', job_department = '" + s7.Trim() + "', job_street = '" + s8.Trim() + "', job_zip = '" + s9.Trim() + "', job_country = '" + s10.Trim() + "', job_city = '" + s11.Trim() + "', job_phone_1 = '" + s12.Trim() + "', job_phone_2 = '" + s13.Trim() + "', job_fax = '" + s14.Trim() + "', job_email = '" + s15.Trim() + "', job_www = '" + s16.Trim() + "', job_skype = '" + s17.Trim() + "', comment = '" + s18.Trim() + "', groups = (select id from groups where name='" + s19.Trim() + "'), job_function = '" + s20.Trim() + "' where id = " + cid.Trim(), _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten!\nBitte überprüfen Sie die Eingabe!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void changeContacts2(String moduserid, String institution, String salutation_id, String title_id, String firstname, String lastname, String job_company, String job_department, String job_street, String job_zip, String job_country, String job_city, String job_phone_1, String job_email, String comment, String job_function, string id)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("update contacts set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', lastuser_id = '" + moduserid.Trim() + "', institution = '" + institution + "', salutation_id = " + salutation_id + ", title_id = " + title_id + ", firstname = '" + firstname.Trim() + "', lastname = '" + lastname.Trim() + "', job_company = '" + job_company.Trim() + "', job_department = '" + job_department.Trim() + "', job_street = '" + job_street.Trim() + "', job_zip = '" + job_zip.Trim() + "', job_country = '" + job_country.Trim() + "', job_city = '" + job_city.Trim() + "', job_phone_1 = '" + job_phone_1.Trim() + "', job_email = '" + job_email.Trim() + "', comment = '" + comment.Trim() + "', job_function = '" + job_function.Trim() + "' where id = " + id.Trim(), _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten!\nBitte überprüfen Sie die Eingabe!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void deleteContacts(String id)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("delete from contacts where id = '" + id + "';", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Fehler beim Löschen!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<string> fillTitleIntoContacts()
        {
            try
            {
                List<string> fillTitle = new List<string>();

                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select name from titles", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader["name"].ToString() != null && myReader["name"].ToString() != "")
                    {
                        fillTitle.Add(myReader["name"].ToString());
                    }
                }
                myReader.Close();
                return fillTitle;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<Title> gettitel()
        {
            try
            {
                List<Title> fillTitle = new List<Title>();

                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select id, name from titles order by id", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    fillTitle.Add( new Title (myReader["id"].ToString(), myReader["name"].ToString()));
                }
                myReader.Close();
                return fillTitle;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<string> fillGroupsIntoContacts()
        {
            try
            {
                List<string> fillGroups = new List<string>();

                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select name from groups", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader["name"].ToString() != null && myReader["name"].ToString() != "")
                    {
                        fillGroups.Add(myReader["name"].ToString());
                    }
                }
                myReader.Close();
                return fillGroups;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal List<Contacts> fillContacts()
        {
            List<Contacts> list = new List<Contacts>();
            String id, salutation, institution, title, firstname, lastname, company, department, street, zip, country, city, phone1, phone2, fax, email, www, skype, comment, groups, function;

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("select c.id, s.name as salutation, t.name as title, firstname, lastname, institution, job_company, job_department, job_country, job_zip, job_city, job_street, job_phone_1, job_phone_2, job_fax, job_email, job_www, job_skype, comment, g.name as groups, job_function from contacts c left join salutations s on c.salutation_id=s.id left join titles t on c.title_id=t.id left join groups g on c.groups=g.id", _myConnection);
                MySqlDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    id = (myReader["id"].ToString());
                    salutation = (myReader["salutation"].ToString());
                    title = (myReader["title"].ToString());
                    firstname = (myReader["firstname"].ToString());
                    lastname = (myReader["lastname"].ToString());
                    institution = (myReader["institution"].ToString());
                    company = (myReader["job_company"].ToString());
                    department = (myReader["job_department"].ToString());
                    country = (myReader["job_country"].ToString());
                    zip = (myReader["job_zip"].ToString());
                    city = (myReader["job_city"].ToString());
                    street = (myReader["job_street"].ToString());
                    phone1 = (myReader["job_phone_1"].ToString());
                    phone2 = (myReader["job_phone_2"].ToString());
                    fax = (myReader["job_fax"].ToString());
                    email = (myReader["job_email"].ToString());
                    www = (myReader["job_www"].ToString());
                    skype = (myReader["job_skype"].ToString());
                    comment = (myReader["comment"].ToString());
                    groups = (myReader["groups"].ToString());
                    function = (myReader["job_function"].ToString());

                    list.Add(new Contacts(id, salutation, title, firstname, lastname, institution, company, department, country, zip, city, street, phone1, phone2, fax, email, www, skype, comment, groups, function));
                }
                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _myConnection.Close();
            }

            return list;
        }

        internal List<Contacts> getSozialarbeiter()
        {
            List<Contacts> list = new List<Contacts>();
            String id, salutation, institution, title, firstname, lastname, company, department, street, zip, country, city, phone1, phone2, fax, email, www, skype, comment, groups, function;

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("select c.id, s.name as salutation, t.name as title, firstname, lastname, institution, job_company, job_department, job_country, job_zip, job_city, job_street, job_phone_1, job_phone_2, job_fax, job_email, job_www, job_skype, comment, g.name as groups, job_function from contacts c left join salutations s on c.salutation_id=s.id left join titles t on c.title_id=t.id left join groups g on c.groups=g.id where job_function LIKE  '%sozialarbeiter%'", _myConnection);
                MySqlDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    id = (myReader["id"].ToString());
                    salutation = (myReader["salutation"].ToString());
                    title = (myReader["title"].ToString());
                    firstname = (myReader["firstname"].ToString());
                    lastname = (myReader["lastname"].ToString());
                    institution = (myReader["institution"].ToString());
                    company = (myReader["job_company"].ToString());
                    department = (myReader["job_department"].ToString());
                    country = (myReader["job_country"].ToString());
                    zip = (myReader["job_zip"].ToString());
                    city = (myReader["job_city"].ToString());
                    street = (myReader["job_street"].ToString());
                    phone1 = (myReader["job_phone_1"].ToString());
                    phone2 = (myReader["job_phone_2"].ToString());
                    fax = (myReader["job_fax"].ToString());
                    email = (myReader["job_email"].ToString());
                    www = (myReader["job_www"].ToString());
                    skype = (myReader["job_skype"].ToString());
                    comment = (myReader["comment"].ToString());
                    groups = (myReader["groups"].ToString());
                    function = (myReader["job_function"].ToString());

                    list.Add(new Contacts(id, salutation, title, firstname, lastname, institution, company, department, country, zip, city, street, phone1, phone2, fax, email, www, skype, comment, groups, function));
                }
                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _myConnection.Close();
            }

            return list;
        }

        #endregion

        public String getServiceIdbyClientId(String id)
        {
            try
            {
                List<Contacts> contacts = new List<Contacts>();
                String cid = "";
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select service_id from clientstoservices where client_id=" + id + ";", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    cid = myReader["service_id"].ToString();
                }
                myReader.Close();
                return cid;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<Service> getServicesVital()
        {
            try
            {
                List<Service> services = new List<Service>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select id , name from services;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String id = myReader["id"].ToString();
                    String name = myReader["name"].ToString();
                    services.Add(new Service(id, name));
                }
                myReader.Close();
                return services;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public bool setVital(String sex, String firstname, String lastname, String inclusion, String date_of_birth, String citizenship, String district_authority, String insurance, String icd, String place_of_birth, String social_insurance_number, String co_insured, String contact_id, String service_id, String status, String assignment)
        {
            string check = null;
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("insert into clients(sex, firstname, lastname, inclusion, date_of_birth, citizenship, district_authority, insurance, icd, place_of_birth, social_insurance_number, co_insured, contact_id, status, assignment) values(" + sex + ",'" + firstname + "','" + lastname + "','" + inclusion + "','" + date_of_birth + "','" + citizenship + "','" + district_authority + "','" + insurance + "','" + icd + "','" + place_of_birth + "','" + social_insurance_number + "','" + co_insured + "'," + contact_id + ",'" + status + "','" + assignment + "');", _myConnection);
                myCommand.ExecuteNonQuery();
                _myConnection.Close();
                _myConnection.Open();
                String clientid = "";
                myCommand = new MySqlCommand("select max(id) abc from clients", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    clientid = myReader["abc"].ToString();
                }
                _myConnection.Close();
                _myConnection.Open();
                myCommand = new MySqlCommand("insert into clientstoservices(client_id,service_id) values(" + clientid + " , " + service_id + ");", _myConnection);
                myCommand.ExecuteNonQuery();
                


                check = clientid;

                if (check != null)
                {
                    FtpHandler ftp = new FtpHandler();
                    ftp.CreatePathForClient(check);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                return false;
            }
            finally
            {
                _myConnection.Close();
            }
            return true;
        }

        public void changeVital(String id, String sex, String firstname, String lastname, String inclusion, String date_of_birth, String citizenship, String district_authority, String insurance, String icd, String place_of_birth, String social_insurance_number, String co_insured, String contact_id, String service_id, String leaving, String status, String assignment)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommannd;
                if (String.IsNullOrEmpty(leaving.Trim()))
                {
                    myCommannd = new MySqlCommand("update clients set sex=" + sex + " , firstname='" + firstname + "' , lastname='" + lastname + "' , date_of_birth='" + date_of_birth + "' , inclusion='" + inclusion + "' , citizenship='" + citizenship + "' , district_authority='" + district_authority + "' , insurance='" + insurance + "' , icd='" + icd + "' , place_of_birth='" + place_of_birth + "' , social_insurance_number='" + social_insurance_number + "' , co_insured='" + co_insured + "' , contact_id=" + contact_id + " , status='" + status + "' , assignment='" + assignment + "' where id=" + id + ";", _myConnection);
                }
                else
                {
                    myCommannd = new MySqlCommand("update clients set sex=" + sex + " , firstname='" + firstname + "' , lastname='" + lastname + "' , date_of_birth='" + date_of_birth + "' , inclusion='" + inclusion + "' , citizenship='" + citizenship + "' , district_authority='" + district_authority + "' , insurance='" + insurance + "' , icd='" + icd + "' , place_of_birth='" + place_of_birth + "' , social_insurance_number='" + social_insurance_number + "' , co_insured='" + co_insured + "' , contact_id=" + contact_id + " , leaving=" + leaving + " , status='" + status + "' , assignment='" + assignment + "' where id=" + id + ";", _myConnection);
                }
                
                myCommannd.ExecuteNonQuery();
                _myConnection.Close();
                _myConnection.Open();
                myCommannd = new MySqlCommand("update clientstoservices set service_id=" + service_id + " where client_id=" + id + ";", _myConnection);
                myCommannd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<BodyInfo> getBodyInfo(String id)
        {
            try
            {
                List<BodyInfo> infos = new List<BodyInfo>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select created, size, weight from clientsvitalstats where client_id=" + id + " order by created desc ;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String date = myReader["created"].ToString();
                    String size = myReader["size"].ToString();
                    String weight = myReader["weight"].ToString();
                    //services.Add(new Service(id, name));
                    //String weight = myReader["weight"].ToString();
                    infos.Add(new BodyInfo(date, size, weight));
                }
                myReader.Close();
                return infos;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setBodyInfo(String id, String size, String weight)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommannd = new MySqlCommand("insert into clientsvitalstats(client_id,size,weight,created) values(" + id + " , " + size + " , " + weight + " , curDate() );", _myConnection);
                myCommannd.ExecuteNonQuery();
            }
            catch 
            {
                MessageBox.Show("Es gab einen Feher beim eintragen der Werte überprüfen sie ihre Eingabe. \n Die Zahlen müssn in diesem Format angegeben werden 0.0 ",
                    "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setnewMedicalActionType(String type)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommannd = new MySqlCommand("insert into medicalactions(name) values('" + type + "');", _myConnection);
                myCommannd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<Art> geileMethode()
        {
            try
            {
                List<Art> infos = new List<Art>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select id,name from medicalactions;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String id = myReader["id"].ToString();
                    String name = myReader["name"].ToString();
                    infos.Add(new Art(id, name));
                }
                myReader.Close();
                return infos;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getMediActionIDbyName(string name)
        {
            try
            {
                String id = "";
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select id from medicalactions where name = '"+ name + "';", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    id = myReader["id"].ToString();
                }
                myReader.Close();
                return id;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void WasSollDieseMethodeKoennen(String id, String cid, String date, String desc, String artid)
        {
            try
            { //c.WasSollDieseMethodeKoennen(id, hoergeraetstrahlenangriff12345, date, txtdesc.Text, cmbArt.SelectedValue.ToString());
                _myConnection.Open();
                MySqlCommand myCommannd = new MySqlCommand("insert into clientsmedicalactions(client_id,createuser_id,created,realized,medicalaction_id,statement) values(" + cid + " , " + id + " , curDate() , '" + date + "' , " + artid + " , '" + desc + "');", _myConnection);
                myCommannd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<MediAkt> GetClientMed(String rudi)
        {
            try
            {
                List<MediAkt> infos = new List<MediAkt>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select medicalactions.name a, clientsmedicalactions.realized b, clientsmedicalactions.statement c from clientsmedicalactions JOIN medicalactions ON medicalactions.id = clientsmedicalactions.medicalaction_id where client_id=" + rudi + " order by realized desc;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String name = myReader["a"].ToString();
                    String date = myReader["b"].ToString();
                    String desc = myReader["c"].ToString();
                    infos.Add(new MediAkt(date, name, desc));
                }
                myReader.Close();
                return infos;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<MediAkt> GetClientMed_Month(String rudi, string month, string year)
        {
            try
            {
                List<MediAkt> infos = new List<MediAkt>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select medicalactions.name a, clientsmedicalactions.realized b, clientsmedicalactions.statement c from clientsmedicalactions JOIN medicalactions ON medicalactions.id = clientsmedicalactions.medicalaction_id where client_id=" + rudi + " and month(clientsmedicalactions.realized) = " + month + " and year(clientsmedicalactions.realized) = " + year + " order by realized desc;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String name = myReader["a"].ToString();
                    String date = myReader["b"].ToString();
                    String desc = myReader["c"].ToString();
                    infos.Add(new MediAkt(date, name, desc));
                }
                myReader.Close();
                return infos;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        #endregion

        #region KilometerGeld

        public void setKilometerGeld(String uid, String Kennzeichen, String Ortvon, String Ortbis, String Zeitvon, String Zeitbis, String Summe, String km)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("insert into kilometergeld (User,Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km) Values(" + uid + " , '" + Kennzeichen + "' , '" + Ortvon + "' , '" + Ortbis + "' , '" + Zeitvon + "' , '" + Zeitbis + "' , " + Summe + " , " + km + ");", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch 
            {
                MessageBox.Show("Die Zeit wurde in keinem passenden Format eingeben XX:XX", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<KmG> getKilometerGeld(String uid)
        {
            try
            {
                _myConnection.Open();
                List<KmG> list = new List<KmG>();
                MySqlCommand myCommand = new MySqlCommand("select User,Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km from kilometergeld where User=" + uid, _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String user = myReader["User"].ToString();
                    String kennzeichen = myReader["Kennzeichen"].ToString();
                    String ortvon = myReader["Ortvon"].ToString();
                    String ortbis = myReader["Ortbis"].ToString();
                    String zeitvon = myReader["Zeitvon"].ToString();
                    String zeitbis = myReader["Zeitbis"].ToString();
                    String km = myReader["km"].ToString();
                    String Summe = myReader["Summe"].ToString();
                    list.Add(new KmG(user, kennzeichen, km, ortvon, ortbis, zeitvon, zeitbis, Summe));
                }
                myReader.Close();
                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<KmG> getKilometerGeld_Month(String uid, string month, string year)
        {
            try
            {
                _myConnection.Open();
                List<KmG> list = new List<KmG>();
                MySqlCommand myCommand = new MySqlCommand("select User,Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km from kilometergeld where User=" + uid + " AND MONTH(Zeitvon) = " + month + " AND YEAR(Zeitvon) = " + year, _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String user = myReader["User"].ToString();
                    String kennzeichen = myReader["Kennzeichen"].ToString();
                    String ortvon = myReader["Ortvon"].ToString();
                    String ortbis = myReader["Ortbis"].ToString();
                    String zeitvon = myReader["Zeitvon"].ToString();
                    String zeitbis = myReader["Zeitbis"].ToString();
                    String km = myReader["km"].ToString();
                    String Summe = myReader["Summe"].ToString();
                    list.Add(new KmG(user, kennzeichen, km, ortvon, ortbis, zeitvon, zeitbis, Summe));
                }
                myReader.Close();
                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<KmG> getKilometerGeld(String uid, String month, String year)
        {
            try
            {
                _myConnection.Open();
                List<KmG> list = new List<KmG>();
                MySqlCommand myCommand = new MySqlCommand("select User,Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km from kilometergeld where User=" + uid + " and Month(Zeitvon)='" + month + "' and Year(Zeitvon)='" + year + "'" + " and Month(Zeitbis)='" + month + "' and Year(Zeitbis)='" + year + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    String user = myReader["User"].ToString();
                    String kennzeichen = myReader["Kennzeichen"].ToString();
                    String ortvon = myReader["Ortvon"].ToString();
                    String ortbis = myReader["Ortbis"].ToString();
                    String zeitvon = myReader["Zeitvon"].ToString();
                    String zeitbis = myReader["Zeitbis"].ToString();
                    String km = myReader["km"].ToString();
                    String Summe = myReader["Summe"].ToString();
                    list.Add(new KmG(user, kennzeichen, km, ortvon, ortbis, zeitvon, zeitbis, Summe));
                }
                myReader.Close();
                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        #endregion

        internal void setMediForDay(string p, DateTime when, Medicaments medi, string id, string why)
        {
            try
            {
                string[] name = p.Split(' ');
                _myConnection.Open();
                string clientId = "";

                MySqlCommand myCommand = new MySqlCommand("select id from clients where firstname ='"
                    + name[0].Replace(',', ' ') + "' and lastname ='" + name[1].Replace(',', ' ') + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    clientId = myReader["id"].ToString();
                }



                _myConnection.Close();
                _myConnection.Open();


                myCommand = new MySqlCommand("insert into clientsmedicationsconfirmations (created, createuser_id, client_id, for_day,clientsmedication_id, morning, midday ,evening ,night,confirmed,reason_confirmed) " +
                    "values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "'," + id + ",'" + clientId + "','" + when.ToString("yyyy-MM-dd") + "','" + medi.cmId + "','" + boolToInt(medi.morningConfirmed) + "','" + boolToInt(medi.middayConfirmed) + "','" + boolToInt(medi.eveningConfirmed) + "','" + boolToInt(medi.nightConfirmed) + "','" + "1" + "','" + why + "')", _myConnection);
                int test = myCommand.ExecuteNonQuery();
                myReader.Close();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        private int boolToInt(bool p)
        {
            if (p)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void renameWiki(WikiDoc tmp, string title, User u)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("update wiki set lastuser_id = " + u.Id + " , title='" + title + "' where id = " + tmp.client_id + " ;", _myConnection);

                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getPW(string p)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select pwThera from users u where u.id = " + p, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["pwThera"].ToString();
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void setNewPW(string pw, string id)
        {
            try
            {

                _myConnection.Open();


                MySqlCommand myCommand = new MySqlCommand("Update users set pwThera = '" + pw + "' where id = " + id, _myConnection);
                int test = myCommand.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

        }

        internal string[] userToService(string p)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select s.name from users u join userstoservices us on u.id = us.user_id join services s on us.service_id=s.id where u.id = " + p, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["name"].ToString();
                    ret += "$";
                }

                myReader.Close();

                return ret.Split('$');
            }
            catch 
            {
                /**/
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal bool isAdmin(string p)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select isAdmin from users u where u.id = " + p, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["isAdmin"].ToString();

                }

                myReader.Close();

                return Convert.ToBoolean(ret);
            }
            catch 
            {
                /**/
                return false;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void AddUserstoServices(int user_id, List<Service> service_id)
        {
            try
            {

                _myConnection.Open();
                string command = "";

                foreach (Service serv in service_id)
                {
                    command += "insert into userstoservices (user_id, service_id) values (" + user_id + ", " + serv.Id + " ); ";
                }

                MySqlCommand myCommand = new MySqlCommand(command, _myConnection);
                myCommand.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }



        internal string getStammdatenById(int p)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select firstname, lastname, username, social_insurance_number, street, zip, city, phone_1, fax, bank, bank_code, bank_account_number, email_user, email_password, weeklyHours, weeklyDays, isAdmin, date_of_birth,inclusion,leaving from users where users.id = " + p, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["firstname"].ToString() + "$";
                    ret += myReader["lastname"].ToString() + "$";
                    ret += myReader["username"].ToString() + "$";
                    ret += myReader["social_insurance_number"].ToString() + "$";
                    ret += myReader["street"].ToString() + "$";
                    ret += myReader["zip"].ToString() + "$";
                    ret += myReader["city"].ToString() + "$";
                    ret += myReader["phone_1"].ToString() + "$";
                    ret += myReader["fax"].ToString() + "$";
                    ret += myReader["bank"].ToString() + "$";
                    ret += myReader["bank_code"].ToString() + "$";
                    ret += myReader["bank_account_number"].ToString() + "$";
                    ret += myReader["email_user"].ToString() + "$";
                    ret += myReader["email_password"].ToString() + "$";
                    ret += myReader["weeklyHours"].ToString() + "$";
                    ret += myReader["weeklyDays"].ToString() + "$";
                    ret += myReader["isAdmin"].ToString() + "$";
                    ret += myReader["date_of_birth"].ToString() + "$";
                    ret += myReader["inclusion"].ToString() + "$";
                    ret += myReader["leaving"].ToString() + "$";
                }
                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void setNewUser(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16, string p17, string p18, string p19, string p20, DateTime p21)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("INSERT INTO users(bank_account_number, firstname , lastname, bank , bank_code , city , fax , email_password, email_address , email_user , street , social_insurance_number , phone_1 , username , weeklyHours , weeklyDays , zip , pwThera , createuser_id , lastuser_id , date_of_birth , isAdmin, inclusion) VALUES ('" + p1 + "' , '" + p2 + "' ,'" + p3 + "' ,'" + p4 + "' ,'" + p5 + "' ,'" + p6 + "' ,'" + p7 + "' ,'" + p8 + "' ,'" + p9 + "', '" + p9 + "' ,'" + p10 + "' ,'" + p11 + "' ,'" + p12 + "' ,'" + p13 + "' ,'" + p14 + "' ,'" + p15 + "' ,'" + p16 + "' ,'" + p17 + "','" + p18 + "','" + p18 + "','" + Convert.ToDateTime(p19).ToString("yyyy-MM-dd HH:mm") + "','" + p20 + "', '" + p21.ToString("yyyy-MM-dd HH:mm") + "');", _myConnection);
                myCommand.ExecuteNonQuery();



            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

            //AddUserstoServices(Convert.ToInt32(getUserIDbyFullname(p2 + " " + p3)), Convert.ToInt32(getServicesIdByName(p21)));

        }

        private string getServicesIdByName(string p21)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select id from services where name = '" + p21 + "'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret = myReader["id"].ToString();
                }
                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setEmailPass(string id, string emailpass)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("update users set email_password='" + emailpass + "' where id = '" + id + "' ; ", _myConnection);

                myCommand.ExecuteNonQuery();
                MessageBox.Show("Erfolg");
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler");
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setDropboxPass(string id, string pass)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("update users set Dropboxpw='" + pass + "' where id = '" + id + "' ; ", _myConnection);

                myCommand.ExecuteNonQuery();
                MessageBox.Show("Erfolg");
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler");
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void updateUser(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16, string p17, string p18, string p19, int p20, string p21,string p22,string p23)
        {
            try
            {
                _myConnection.Open();
                
                
                MySqlCommand myCommand = new MySqlCommand("update users set bank_account_number='" + p1 + "', firstname='" + p2 + "' , lastname='" + p3 + "', bank='" + p4 + "' , bank_code='" + p5 + "' , city='" + p6 + "' , fax='" + p7 + "' , email_password='" + p8 + "' , email_user='" + p9 + "' , street='" + p10 + "' , social_insurance_number='" + p11 + "' , phone_1='" + p12 + "' , username='" + p13 + "' , weeklyHours='" + p14 + "' , weeklyDays='" + p15 + "' , zip='" + p16 + "' , pwThera='" + p17 + "' , lastuser_id='" + p18 + "', isAdmin='" + p21 + "' , date_of_birth='" + p19 + "' , leaving='" + p22 + "' , inclusion='" + p19 + "' where id = '" + p20 + "' ; ", _myConnection);

                myCommand.ExecuteNonQuery();



            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public String getIdbyName(String name)
        {
            {
                try
                {
                    _myConnection.Open();
                    String id = "";
                    String[] ar = name.Split(' ');
                    MySqlCommand command = new MySqlCommand("select id from users where firstname='" + ar[0] + "' and lastname='" + ar[1] + "' and leaving is null;", _myConnection);


                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader["id"].ToString();
                    }
                    reader.Close();
                    return id;

                }
                catch (Exception e)
                {
                    return e.ToString(); ;
                }
                finally
                {
                    _myConnection.Close();
                }
            }
        }

        public String getIdbyNameClients(String kid)
        {
            {
                try
                {
                    string[] name = kid.Split(' ');
                    _myConnection.Open();
                    String id = "";
                    MySqlCommand myCommand = new MySqlCommand("select id from clients where firstname ='"
                        + name[0].Replace(',', ' ') + "' and lastname ='" + name[1].Replace(',', ' ') + "'", _myConnection);
                    MySqlDataReader myReader = null;
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        id = myReader["id"].ToString();
                    }
                    return id;

                }
                catch (Exception e)
                {
                    return e.ToString(); ;
                }
                finally
                {
                    _myConnection.Close();
                }
            }
        }


        internal void updateUserNoPw(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16, string p17, string p18, string p19, int p20,DateTime p21, DateTime p22)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("update users set bank_account_number='" + p1 + "', firstname='" + p2 + "' , lastname='" + p3 + "', bank='" + p4 + "' , bank_code='" + p5 + "' , city='" + p6 + "' , fax='" + p7 + "' , email_password='" + p8 + "' , email_user='" + p9 + "' , street='" + p10 + "' , social_insurance_number='" + p11 + "' , phone_1='" + p12 + "' , username='" + p13 + "' , weeklyHours='" + p14 + "' , weeklyDays='" + p15 + "' , zip='" + p16 + "', lastuser_id='" + p17 + "', isAdmin='" + p19 + "', inclusion='" + Convert.ToDateTime(p22).ToString("yyyy-MM-dd HH:mm") + "', leaving='" + Convert.ToDateTime(p21).ToString("yyyy-MM-dd HH:mm") + "' , date_of_birth='" + p18 + "' where id = '" + p20 + "' ; ", _myConnection);

                myCommand.ExecuteNonQuery();



            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

            //updateUsertoService(Convert.ToInt32(getUserIDbyFullname(p2 + " " + p3)), Convert.ToInt32(getServicesIdByName(p21)));
        }

        internal void updateUserNoPw(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16, string p17, string p18, string p19, int p20, DateTime p22)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("update users set bank_account_number='" + p1 + "', firstname='" + p2 + "' , lastname='" + p3 + "', bank='" + p4 + "' , bank_code='" + p5 + "' , city='" + p6 + "' , fax='" + p7 + "' , email_password='" + p8 + "' , email_user='" + p9 + "' , street='" + p10 + "' , social_insurance_number='" + p11 + "' , phone_1='" + p12 + "' , username='" + p13 + "' , weeklyHours='" + p14 + "' , weeklyDays='" + p15 + "' , zip='" + p16 + "', lastuser_id='" + p17 + "', isAdmin='" + p19 + "', inclusion='" + Convert.ToDateTime(p22).ToString("yyyy-MM-dd HH:mm") + "', date_of_birth='" + p18 + "' where id = '" + p20 + "' ; ", _myConnection);

                myCommand.ExecuteNonQuery();



            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

            //updateUsertoService(Convert.ToInt32(getUserIDbyFullname(p2 + " " + p3)), Convert.ToInt32(getServicesIdByName(p21)));
        }

        public void updateUsertoService(int p, List<Service> p_2)
        {

            try
            {

                _myConnection.Open();

                MySqlCommand myCommand = new MySqlCommand("delete from userstoservices where user_id = " + p, _myConnection);
                myCommand.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }

            AddUserstoServices(Convert.ToInt32(p), p_2);
        }

        internal string getWorkingDays(string id)
        {
            try
            {

                _myConnection.Open();
                string gaaaah = "";
                MySqlCommand myCommand = new MySqlCommand("select weeklyDays from users where id = " + id, _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    gaaaah = myReader["weeklydays"].ToString();
                }
                return gaaaah;

            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setTaschengeld(string tgKlient, string diff, char zeichen, string com, string name)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;

                myCommand = new MySqlCommand("insert into taschengeld(Client_ID, TG_before, TG_diff, TG_after, Comment, Name, Date) values(" + tgKlient + ", " +
                    "(select pocket_money from clients where id = " + tgKlient + "), " +
                    diff + ", " +
                    "(select pocket_money from clients where id = " + tgKlient + ") " + zeichen.ToString() + " " + diff + ", '" +
                    com + "' , '" +
                    name + "' , '" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')", _myConnection);
                myCommand.ExecuteNonQuery();

                myCommand = new MySqlCommand("update clients set pocket_money = pocket_money" + zeichen.ToString() + diff + " where id = " + tgKlient, _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getTaschengeld(string id)
        {
            try
            {
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select pocket_money from clients where id = " + id, _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg = myTgReader["pocket_money"].ToString();
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal List<Taschengeld> getTaschengeldDoku(string id)
        {
            try
            {
                List<Taschengeld> tglist = new List<Taschengeld>();
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select Name, Date, TG_before, TG_diff, TG_after, Comment from taschengeld where Client_ID = " + id + " order by ID desc", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    String name = myTgReader["Name"].ToString();
                    String date = myTgReader["Date"].ToString();
                    double before = Convert.ToDouble(myTgReader["TG_before"].ToString());
                    String diff = myTgReader["TG_diff"].ToString();
                    double after = Convert.ToDouble(myTgReader["TG_after"].ToString());
                    String com = myTgReader["Comment"].ToString();
                    double nachher = after - before;
                    bool in_out = false;
                    if (nachher > -1)
                    {
                        in_out = true;
                    }
                    if (in_out)
                    {
                        tglist.Add(new Taschengeld(name, date, nachher.ToString(), "-", after.ToString(), com));
                    }
                    else
                    {
                        tglist.Add(new Taschengeld(name, date, "-", (nachher * (-1)).ToString(), after.ToString(), com));
                    }
                }

                return tglist;
            }
            catch
            {
                /**/
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getFVGs(string p1, string p2)
        {
            try
            {
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 + "' and lastname='" + p2 + "') and art=3", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"].ToString() + "$";
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }


        internal string getFVGValue(string name, string p, string p2)
        {
            try
            {
                string[] datum = p2.Split(' ')[0].Split('.');
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select * from clientsfvgs c where day(c.from) = '" + datum[0] + "' and month(c.from) = '" + datum[1] + "' and year(c.from) = '" + datum[2] + "' and client_id = (select id from clients where firstname='" + name + "' and lastname='" + p + "') ", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["content"].ToString();
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getFVGValue2(string name, string p, string p2, string art)
        {
            try
            {
                string[] datum = p2.Split(' ')[0].Split('.');
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select * from clientsfvgs c where day(c.from) = '" + datum[0] + "' and month(c.from) = '" + datum[1] + "' and year(c.from) = '" + datum[2] + "' and client_id = (select id from clients where firstname='" + name + "' and lastname='" + p + "') ", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["content"].ToString();
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getBericht_Content(Klienten_Berichte ber)
        {
            try
            {
                string command = "";
                if (ber.table == 1)
                {
                    command = "select content from clientsfvgs where id = " + ber.id;
                }
                else
                {
                    command = "select content from clientsreports where id = " + ber.id;
                }
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["content"].ToString();
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void addBericht(Klienten_Berichte ber, User u, string datetime)
        {
            try
            {
                string command = ""; //swag

                command = "insert into clientsreports (created, modified, createuser_id, client_id, name, content, art) values ('" + datetime + "', '" + datetime + "' , " + u.Id + ", " + ber.Client_id + ", '" + ber.name + "', '" + ber.content + "', " + ber.art + " );";
                
                
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
                
                myTg.ExecuteNonQuery();

              
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void addBericht_Vorlage(Klienten_Berichte ber)
        {
            try
            {
                string command = ""; //swag

                command = "insert into vorlagen_bericht (name, content) values ('" + ber.name +"', '" + ber.content +"');";

                
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
                
                myTg.ExecuteNonQuery();

             
            }
            catch
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void setBericht_Content(Klienten_Berichte ber, User u)
        {
            try
            {
                string command = "";
                if (ber.table == 1)
                {
                    command = "update clientsfvgs set content = '" + ber.content + "', modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', lastuser_id = " + u.Id +" where id = " + ber.id;
                }
                else
                {
                    command = "update clientsreports set content = '" + ber.content + "', modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', lastuser_id = " + u.Id + " where id = " + ber.id;
                }
                
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
               myTg.ExecuteNonQuery();
               MessageBox.Show("Gespeichert");
            }
            catch
            {
                MessageBox.Show("Fehler beim Speichern!");
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void setBericht_Art(Klienten_Berichte ber)
        {
            try
            {
                string command = "";
                if (ber.table == 1)
                {
                    command = "update clientsfvgs set art = " + ber.art + " where id = " + ber.id;
                }
                else
                {
                    command = "update clientsreports set art = " + ber.art + " where id = " + ber.id;
                }
                
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
                myTg.ExecuteNonQuery();
                MessageBox.Show("Gespeichert");
            }
            catch 
            {
                MessageBox.Show("Fehler beim Speichern!");
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal List<Klienten_Berichte> getBericht_Vorlage()
        {
            List<Klienten_Berichte> ret = new List<Klienten_Berichte>();
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand();
                
                myCommand = new MySqlCommand("Select id, name, content from vorlagen_bericht", _myConnection);
                
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    Klienten_Berichte tmp = new Klienten_Berichte();
                    tmp.id = Convert.ToInt32(myReader["id"].ToString());
                    tmp.name = myReader["name"].ToString();
                    tmp.content = myReader["content"].ToString();

                    ret.Add(tmp);
                }

                myReader.Close();

                return ret;
            }
            catch 
            {
                /**/
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void InsertFirstPMoney(string uid)
        {
            List<string> id = new List<string>();
            List<string> value = new List<string>();
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT id, pocket_money FROM clients", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    id.Add(myReader["id"].ToString());
                    value.Add(myReader["pocket_money"].ToString());
                }
                myReader.Close();
                for (int i = 0; i < id.Count; i++)
                {
                    myCommand = new MySqlCommand("insert into pocket_money (ID, val, comment, eintrUserID, datum) values (\'" + id[i] + "\', \'" + value[i] + "\', 'Anfangsbestand des Taschengeldes', " + uid + ", CURDATE())", _myConnection);
                    myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getName(int user, DateTime from, DateTime to, string comment)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select von FROM workingtime WHERE usersid='" + user + "' AND art = 'Urlaub' AND holidayverfied = 1 AND DAY(datetimefrom)=" + from.Day + " AND MONTH(datetimefrom)=" + from.Month + " AND YEAR(datetimefrom)=" + from.Year + " AND DAY(datetimeto)=" + to.Day + " AND MONTH(datetimeto)=" + to.Month + " AND YEAR(datetimeto)=" + to.Year + " AND comment ='" + comment + "' AND MINUTE(datetimefrom)=" + from.Minute + " AND HOUR(datetimefrom)=" + from.Hour + " AND MINUTE(datetimeto)=" + to.Minute + " AND HOUR(datetimeto)=" + to.Hour, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret = myReader["von"].ToString();
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void updatePath(Microsoft.Win32.OpenFileDialog ofd)
        {

        }

        internal void addpath(string filename, int creater, int client, string titel, int size)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("insert into clientsdocuments (client_id, created, modified, createuser_id, title, path, filesize) values (" + client + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', " + creater + ", '" + titel + "', '/data/clients/" + client + "/documents/" + filename + "', " + size + ");", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setHouse(String ID, String name, String street, String zip, String city, String tel, String email, String homepage, String start)
        {
            try
            {
                string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                _myConnection.Open();
                MySqlCommand myCommand;
                myCommand = new MySqlCommand("insert into services (created, modified, createuser_id, lastuser_id, name, street, zip, city, phone_2, email_address, home_page, start) values('" + date + "' , '" + date + "' , '" + ID + "' , '" + ID + "' , '" + name + "' , '" + street + "' , '" + zip + "' , '" + city + "' , '" + tel + "' , '" + email + "' , '" + homepage + "' , '" + start + "');", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal bool getKilometerGeldset(int id, int year, int month)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("SELECT User FROM `kilometergeld` WHERE YEAR(Zeitvon) = " + year + " AND MONTH(Zeitvon) = " + month + " AND User = " + id + " ;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    ret = myReader["User"].ToString();
                }

                myReader.Close();

                if (ret != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                /**/
                return false;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setlogin()
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("insert into login (pcname, datum) values ('"+ System.Environment.MachineName +"','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "');", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void addwiki(string filename, int creater, string titel)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("insert into wiki (created, modified, createuser_id, title, path) values ('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', " + creater + ", '" + titel + "', '/data/wiki/" + filename + "');", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        #region Kassabuch

        public List<Haus> getKBHaeuser()
        {
            List<Haus> ret = new List<Haus>();
            string id, name;
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT id, name FROM services", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    id = (myReader["id"].ToString());
                    name = (myReader["name"].ToString());
                    if (id != "" && name != "")
                    {
                        ret.Add(new Haus(id, name));
                    }

                }
                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + e.Message);
            }
            finally
            {
                _myConnection.Close();
            }

            return ret;
        }

        public List<KontoNr> getKBKontoNr(string hid)
        {
            List<KontoNr> ret = new List<KontoNr>();
            string id, knr, desc;

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT ID, KontoNr, Beschr FROM KontoNr WHERE HID = " + hid + " ORDER BY KontoNr ASC", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    id = (myReader["ID"].ToString());
                    knr = (myReader["KontoNr"].ToString());
                    desc = (myReader["Beschr"].ToString());
                    if (id != "" && knr != "" && desc != "")
                    {
                        ret.Add(new KontoNr(id, knr, desc));
                    }

                }
                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + e.Message);
            }
            finally
            {
                _myConnection.Close();
            }

            return ret;
        }

        public List<KassaBuchNode> getKBEintr(string hid, string from, string to)
        {
            List<KassaBuchNode> ret = new List<KassaBuchNode>();
            string id, belnr, knr, beschr, datum, brutto, steuers, netto, mwst, kassst, user, haus;

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT Kassabuch.ID, BelNr, Datum, Bezeichn, Brutto, Steuers, Netto, MWST, KontoNr.KontoNr, users.firstname, users.lastname, services.name FROM Kassabuch JOIN users ON Kassabuch.EintrUserID = users.id JOIN services ON Kassabuch.Haus = services.id JOIN KontoNr ON Kassabuch.KontoNr = KontoNr.ID WHERE Haus = " + hid + " AND Kassabuch.Datum >= '" + from + "' AND Kassabuch.Datum <= '" + to + "' ORDER BY BelNr desc", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    id = (myReader["ID"].ToString());
                    belnr = (myReader["BelNr"].ToString());
                    datum = (myReader["Datum"].ToString());
                    beschr = (myReader["Bezeichn"].ToString());
                    brutto = (myReader["Brutto"].ToString());
                    steuers = (myReader["Steuers"].ToString());
                    netto = (myReader["Netto"].ToString());
                    mwst = (myReader["MWST"].ToString());
                    knr = (myReader["KontoNr"].ToString());
                    user = (myReader["firstname"].ToString()) + " " + (myReader["lastname"].ToString());
                    haus = (myReader["name"].ToString());


                    if (belnr != "" && knr != "" && beschr != "" && brutto != "" && steuers != "" && netto != "" && mwst != "" && knr != "" && user != "" && haus != "")
                    {
                        ret.Add(new KassaBuchNode(id, belnr, DateTime.Parse(datum), beschr, brutto, steuers, netto, mwst, knr, user, haus));
                    }
                }
                myReader.Close();
                for (int i = 0; i < ret.Count; i++)
                {
                    id = ret[i].id.ToString();
                    myCommand = new MySqlCommand("SELECT SUM(Brutto) kassst FROM Kassabuch WHERE ID <= " + id + " AND Haus = " + hid, _myConnection);
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read() && (kassst = myReader["kassst"].ToString()) != "")
                    {
                        ret[i].addKassst(kassst);
                    }
                    myReader.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + e.Message);
            }
            finally
            {
                _myConnection.Close();
            }

            return ret;
        }

        public void addKBEintr(string belnr, string knr, string beschr, string brutto, string steuers, string netto, string mwst, string datum, string uid, string hid)
        {
            string abst = ", ", hk = "\'";
            try
            {
                brutto = brutto.Replace(",", ".");
                netto = netto.Replace(",", ".");
                mwst = mwst.Replace(",", ".");
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("INSERT INTO Kassabuch (BelNr, Datum, Bezeichn, Brutto, Steuers, Netto, MWST, KontoNr, EintrUserID, Haus) VALUES (" +
                belnr + abst + hk + datum + hk + abst + hk + beschr + hk + abst + brutto + abst + steuers + abst + netto + abst + mwst + abst + knr + abst + uid + abst + hid + ")", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void addKBKnr(string knr, string desc, string hid)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("INSERT INTO KontoNr (KontoNr, Beschr, HID) VALUES (\'" + knr + "\', \'" + desc + "\', " + hid + ")", _myConnection);
                myCommand.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public int getHighestKBBelNr(string hid)
        {
            int ret = 0;

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT MAX(BelNr) as BelNr FROM Kassabuch WHERE Haus = " + hid + " AND YEAR(Datum) = YEAR(CURDATE())", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                myReader.Read();
                if (myReader["BelNr"].ToString() == "")
                {
                    ret = 0;
                }
                else
                {
                    ret = Int32.Parse(myReader["BelNr"].ToString());
                }
                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + e.Message);
            }
            finally
            {
                _myConnection.Close();
            }

            return ret;
        }

        public float getKBBallance(string hid)
        {
            float ret = 0;
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT SUM(Brutto) Netto FROM Kassabuch WHERE haus = " + hid + " AND isKey = 0", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    ret = float.Parse(myReader["Netto"].ToString());
                }
                myReader.Close();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
            return ret;
        }

        public float getKBBalanceBeforeDate(DateTime d)
        {
            float ret = 0;

            return ret;
        }

        public int getLastKBYear()
        {
            int ret = 2000;

            return ret;
        }

        public void updateKB(string col, string val, string id)
        {
            if (id == "-1")
            {
                return;
            }
            try
            {
                val = val.Replace(",", ".");
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("UPDATE Kassabuch SET " + col + " = " + val + " WHERE ID = " + id, _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public int getKBKnrIDByKnr(string knr, string hid)
        {
            int ret = -1;
            string s;

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT ID FROM KontoNr WHERE KontoNr = " + knr + " AND HID = " + hid, _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                if (myReader.Read() && (s = myReader["id"].ToString()) != "")
                {
                    ret = Int32.Parse(s);
                }
                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + e.Message);
            }
            finally
            {
                _myConnection.Close();
            }

            return ret;
        }

        public int getKBUserIDByName(string name, string hid)
        {
            int ret = -1;
            string fir, las, s;
            try
            {
                fir = (name.Split(' ')[0]);
                las = (name.Split(' ')[1]);
            }
            catch
            {
                return ret;
            }

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT ID FROM users WHERE firstname = \'" + fir + "\' AND lastname = \'" + las + "\'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                if (myReader.Read() && (s = myReader["id"].ToString()) != "")
                {
                    ret = Int32.Parse(s);
                }
                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + e.Message);
            }
            finally
            {
                _myConnection.Close();
            }

            return ret;
        }

        public float getKBBal1(string hid, string dat)
        {
            float ret = 0;
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT SUM(Brutto) Netto FROM Kassabuch WHERE haus = " + hid + " AND Datum < '" + dat + "' AND isKey = 0", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    ret = float.Parse(myReader["Netto"].ToString());
                }
                myReader.Close();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
            return ret;
        }

        public float getKassstAtDate(string hid, string dat)
        {
            float ret = 0;
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT 1 fggt FROM Kassabuch WHERE (SELECT MAX(Datum) FROM Kassabuch) >= '" + dat + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                if (!myReader.Read() || myReader["fggt"].ToString() == String.Empty)
                {
                    ret = float.NaN;
                    return ret;
                }
                myReader.Close();
                myCommand = new MySqlCommand("SELECT SUM(Netto) Netto FROM Kassabuch WHERE haus = " + hid + " AND Datum < '" + dat + "' AND isKey = 0", _myConnection);
                myReader = null;
                myReader = myCommand.ExecuteReader();

                if (myReader.Read() && myReader["Netto"].ToString() != String.Empty)
                {
                    ret = float.Parse(myReader["Netto"].ToString());
                }
                else
                {
                    ret = 0;
                }
                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                _myConnection.Close();
            }
            return ret;
        }

        //mach ich doch andast
        /*
        public void KBInsertMonthTransaction()
        {
            List<string> haeuser = new List<string>();
            string id;
            try
            {
                //Häuser hernehmen
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT id FROM services", myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    id = (myReader["id"].ToString());
                    if (id != "")
                    {
                        haeuser.Add(id);
                    }
                }
                myReader.Close();
                myConnection.Close();
                myConnection.Open();
                //Häuser durchlaufen
                for (int i = 0; i < haeuser.Count; i++)
                {
                    //(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToString("yyyy-MM-dd")
                    string dat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    string bal = this.getKBBal1(haeuser[i], dat).ToString();
                    myCommand = new MySqlCommand("INSERT INTO Kassabuch (BelNr, Datum, Bezeichn, Brutto, Steuers, Netto, MWST, Haus, isKey) VALUES (-1, " + dat + ", \'Anfangsbestand des Monats\', " + bal + ", 0, " + bal + ", 0, " + haeuser[i] + ", 1;", myConnection);
                    myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + e.Message);
            }
            finally
            {
                myConnection.Close();
            }

        }*/

#endregion

        internal void updatePath(Document doc, string name, int modified_id, int client_id, string titel, int size)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("update clientsdocuments set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' , lastuser_id= " + modified_id + ", path = '/data/clients/" + client_id + "/documents/" + name + "', title = '" + titel + "' , filesize = " + size + " where title= '" + doc.title + "' and path = '" + doc.path + "' and createuser_id = " + doc.createuser_id + " and created = '" + doc.created.ToString("yyyy-MM-dd HH:mm") + "' and client_id = " + doc.client_id + ";", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getKMGSumme(String id, String month, String year)
        {
            string ret = "";

            try
            {
                _myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("select SUM(Summe) as 'Summe' from kilometergeld where User=" + id + " and Month(Zeitvon)='" + month + "' and Year(Zeitvon)='" + year + "'" + " and Month(Zeitbis)='" + month + "' and Year(Zeitbis)='" + year + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    ret = myReader["Summe"].ToString();
                }

                myReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                _myConnection.Close();
            }

            if (ret != "")
            {
                return ret;
            }
            else
            {
                return "0";
            }
        }

        internal string getClientpics(int id)
        {
            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select client_id, created, modified, createuser_id, lastuser_id, title, path, filesize from clientsphotos where client_id = " + id, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["client_id"].ToString() + "$";
                    ret += myReader["created"].ToString() + "$";
                    ret += myReader["modified"].ToString() + "$";
                    ret += myReader["createuser_id"].ToString() + "$";
                    ret += myReader["lastuser_id"].ToString() + "$";
                    ret += myReader["title"].ToString() + "$";
                    ret += myReader["path"].ToString() + "$";
                    ret += myReader["filesize"].ToString();
                    ret += "%";
                }

                myReader.Close();

                return ret;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void addpath_pics(string filename, int creater, int client, string titel, int size)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("insert into clientsphotos (client_id, created, modified, createuser_id, title, path, filesize) values (" + client + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', " + creater + ", '" + titel + "', '/data/clients/" + client + "/documents/" + filename + "', " + size + ");", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void updatePath_pics(Document doc, string name, int modified_id, int client_id, string titel, int size)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("update clientsphotos set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' , lastuser_id= " + modified_id + ", path = '/data/clients/" + client_id + "/documents/" + name + "', title = '" + titel + "' , filesize = " + size + " where title= '" + doc.title + "' and path = '" + doc.path + "' and createuser_id = " + doc.createuser_id + " and created = '" + doc.created.ToString("yyyy-MM-dd HH:mm") + "' and client_id = " + doc.client_id + ";", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void deletePath(Document doc)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("delete from clientsdocuments where title= '" + doc.title + "' and path = '" + doc.path + "' and createuser_id = " + doc.createuser_id + " and created = '" + doc.created.ToString("yyyy-MM-dd HH:mm:ss") + "' and client_id = " + doc.client_id + ";", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void deletePath_pics(Document doc)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("delete from clientsphotos where title= '" + doc.title + "' and path = '" + doc.path + "' and createuser_id = " + doc.createuser_id + " and created = '" + doc.created.ToString("yyyy-MM-dd HH:mm:ss") + "' and client_id = " + doc.client_id + ";", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getBericht(string p1, string p2)
        {
            try
            {
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 + "' and lastname='" + p2 + "') and art=0 ", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"].ToString() + "$";
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }



        internal List<Klienten_Berichte> getBericht_clientsreports(string firstname, string lastname, int art)
        {
            try
            {
                List<Klienten_Berichte> ret = new List<Klienten_Berichte>();
                
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select id, modified, name, created from clientsreports where client_id = (select id from clients where firstname='" + firstname + "' and lastname='" + lastname + "') and art=" + art.ToString(), _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    Klienten_Berichte tmp = new Klienten_Berichte();
                    tmp.id = Convert.ToInt32(myTgReader["id"].ToString());
                    tmp.name = myTgReader["name"].ToString() + " - " + myTgReader["created"].ToString();
                    //tmp.name = myTgReader["name"].ToString();
                    tmp.table = 2;
                    ret.Add(tmp);
                }

                return ret;
            }
            catch 
            {
                /**/
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal List<Klienten_Berichte> getBericht_clientsfvgs(string firstname, string lastname, int art)
        {
            try
            {
                List<Klienten_Berichte> ret = new List<Klienten_Berichte>();
                
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select id, modified, name, created from clientsfvgs where client_id = (select id from clients where firstname='" + firstname + "' and lastname='" + lastname + "') and art=" + art.ToString(), _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    Klienten_Berichte tmp = new Klienten_Berichte();
                    tmp.id = Convert.ToInt32(myTgReader["id"].ToString());
                    tmp.name = myTgReader["name"].ToString() + " - " + myTgReader["created"].ToString();
                    //tmp.name = myTgReader["name"].ToString();
                    tmp.table = 1;
                    ret.Add(tmp);
                }

                return ret;
            }
            catch 
            {
                /**/
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getVorfall(string p1, string p2)
        {
            try
            {
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 + "' and lastname='" + p2 + "') and art=1", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"].ToString() + "$";
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getGespr(string p1, string p2)
        {
            try
            {
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 + "' and lastname='" + p2 + "') and art=2", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"].ToString() + "$";
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getJahres(string p1, string p2)
        {
            try
            {
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 + "' and lastname='" + p2 + "') and art=4", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"].ToString() + "$";
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal string getZwischen(string p1, string p2)
        {
            try
            {
                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 + "' and lastname='" + p2 + "') and art=5", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"].ToString() + "$";
                }

                return tg;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        //Funktioniert nicht, k.a. why; wer lust hat, darf Fehler suchen
        internal bool medexists(string p, DateTime when, Medicaments medi)
        {
            //insert into clientsmedicationsconfirmations (created, createuser_id, client_id, for_day,clientsmedication_id, morning, midday ,evening ,night,confirmed,reason_confirmed)
            try
            {
                string[] name = p.Split(' ');
                _myConnection.Open();
                string clientId = "";

                MySqlCommand myCommand = new MySqlCommand("select id from clients where firstname ='"
                    + name[0].Replace(',', ' ') + "' and lastname ='" + name[1].Replace(',', ' ') + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    clientId = myReader["id"].ToString();
                }



                _myConnection.Close();
                _myConnection.Open();

                string tg = "";
                _myConnection.Open();
                MySqlCommand myTg = new MySqlCommand("select COUNT(*) as 'count' from clientsmedicationsconfirmations where created = " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " and client_id= " + clientId.ToString() + " and clientsmedication_id = " + medi.cmId + " and for_day = " + when.ToString("yyyy-MM-dd") + ";", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg = myTgReader["count"].ToString() + "$";
                }

                if (Convert.ToInt32(tg) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                /**/
                return true;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void setPad(String s1, String s2, String s3, String s4, String s5, String s6)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;

                myCommand = new MySqlCommand("insert into clientssanctions(client_id, created, modified, createuser_id, lastuser_id, sanction, statement, date_from, date_to) values(" + s2 + " , '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:SS") + "' , '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:SS") + "' , " + s1 + " , " + s1 + " , '" + s3 + "' , '" + s4 + "' , '" + s5 + "' , '" + s6 + "')", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal List<PadMas> getPad(String id)
        {
            List<PadMas> list = new List<PadMas>();
            List<PadMas> list2 = new List<PadMas>();
            try
            {
                

                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select created, createuser_id, sanction, statement, date_from, date_to from clientssanctions where client_id=" + id + " order by created desc;", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    String created = myReader["created"].ToString();
                    String from = myReader["createuser_id"].ToString();
                    String mas = myReader["sanction"].ToString();
                    String stat = myReader["statement"].ToString();
                    String datVon = myReader["date_from"].ToString();
                    String datBis = myReader["date_to"].ToString();
                    list.Add(new PadMas(created, from, mas, stat, datVon, datBis));
                }

                myReader.Close();

                
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
            
            foreach (PadMas p in list)
            {
                p.from = getNameByID(p.from);
                list2.Add(p);
            }

            return list2;
        }

        internal string getServicesIdByUserId(int id)
        {

            string ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("Select service_id from userstoservices where user_id = " + id, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["service_id"].ToString();
                    ret += "$";
                }

                myReader.Close();

                return ret;
            }
            catch 
            {
                /**/
                return null;
            }
            finally
            {
                _myConnection.Close();
            }

        }

        internal bool checkWikiName(string p)
        {
            try
            {
                string ret = "";
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select count(*) as 'count' from wiki where title = '" + p + "'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret = myReader["count"].ToString();
                }

                myReader.Close();

                if (Convert.ToInt32(ret.Trim()) > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch 
            {
                /**/
                    /**/
                return false;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void updatewiki(string name, int UserId, WikiDoc doc)
        {
            try
            {
                _myConnection.Open();
                string command = "update wiki set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' , path = '/data/wiki/" + name + "', lastuser_id = " + UserId + " where id = " + doc.client_id + ";";
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
                /**/
                    /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void delwikiDoc(WikiDoc doc)
        {
            //@Kostal
            try
            {
                _myConnection.Open();

                string command = "delete from wiki where title = '" + doc.Name + "' and path = '" + doc.path + "' and modified = '" + doc.Verändert.ToString("yyyy-MM-dd HH:mm") + "' and created = '" + doc.Erstellt.ToString("yyyy-MM-dd HH:mm") + "';";
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
                    /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public int iswikiratingset(string uid, WikiDoc tmp)
        {
            try
            {
                int isset = -1;
                
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select count(*) as 'count' from feedback where userId = " + uid + " and wikiId = " + tmp.client_id + ";", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (Convert.ToInt32(myReader["count"].ToString()) == 0)
                    {
                        isset = 0;
                    }
                    else
                    {
                        isset = 1;
                    }
                }

                myReader.Close();
                return isset;
            }
            catch 
            {
                /**/
                    /**/
                return -1;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public double setWikiRating(string uid, int rating, WikiDoc tmp)
        {
            if (iswikiratingset(uid, tmp) == 1)
            {
                try
                {
                    _myConnection.Open();
                    MySqlCommand myCommand;
                    myCommand = new MySqlCommand("update feedback set rate = " + rating + " where userId = " + uid + " and wikiId = " + tmp.client_id + ";", _myConnection);

                    myCommand.ExecuteNonQuery();
                }
                catch 
                {
                    /**/
                    /**/
                    return -1;
                }
                finally
                {
                    _myConnection.Close();
                }
            }
            else //neuer eintrag
            {
                try
                {
                    _myConnection.Open();
                    MySqlCommand myCommand;
                    myCommand = new MySqlCommand("insert into feedback values (" + uid + ", '', " + rating + ", " + tmp.client_id + ");", _myConnection);

                    myCommand.ExecuteNonQuery();
                }
                catch 
                {
                    /**/
                    /**/
                    return -1;
                }
                finally
                {
                    _myConnection.Close();
                }
            }
            //Wiki-Rating muss noch aktuallisiert werden
            int sum = -1;
            int count = -1;
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select sum(rate) as 'rate', count(*) as 'count' from feedback where wikiId = " + tmp.client_id + ";", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    sum = Convert.ToInt32(myReader["rate"].ToString());
                    count = Convert.ToInt32(myReader["count"].ToString());
                }

                myReader.Close();
                
            }
            catch
            {
                /**/
                    /**/
                return -1;
            }
            finally
            {
                _myConnection.Close();
            }
            if (sum > 0)
            {
                if (count > 0)
                {
                    double rate = sum / count;
                    rate = Math.Round(rate, 2);
                    try
                    {
                        _myConnection.Open();
                        MySqlCommand myCommand;
                        myCommand = new MySqlCommand("update wiki set rate = " + rate + " where id = " + tmp.client_id + ";", _myConnection);

                        myCommand.ExecuteNonQuery();
                        return rate;
                    }
                    catch
                    {
                        /**/
                    /**/
                        return -1;
                    }
                    finally
                    {
                        _myConnection.Close();
                    }
                }
            }
            return -1;
        }

        internal void updateMedi(string p1, string p2, DateTime? nullable1, DateTime? nullable2, string p3, string p4, string p5, string p6, bool p7)
        {
            throw new NotImplementedException();
        }

        internal void syncClientsmedications(clientsmedications clientmed)
        {
            //@Kostal
            try
            {
                _myConnection.Open();

                string command = "insert into clientsmedications (client_id, created, modified, createuser_id, lastuser_id, medicament_id, from, to, apply_date, apply_time, morning, midday, evening, night, cancelled) values (" + clientmed.client_id + ", '" + clientmed.created.ToString("yyyy-MM-dd HH:mm") + "', '" + clientmed.modified.ToString("yyyy-MM-dd HH:mm") + "', " + clientmed.createuser_id + ", " + clientmed.lastuser_id + ", " + clientmed.medicament_id + ", '" + clientmed.from.ToString("yyyy-MM-dd") + "', '" + clientmed.to.ToString("yyyy-MM-dd") + "', '" + clientmed.apply_date.ToString("yyyy-MM-dd") + "', " + clientmed.apply_time + ", " + clientmed.morning + ", " + clientmed.midday + ", " + clientmed.evening + ", " + clientmed.night + ", '" + clientmed.cancelled.ToString("yyyy-MM-dd") + "');";
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
                    /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void syncClientsdailydocs(clientsdailydocs clientsdailydoc)
        {
            //@Kostal
            try
            {
                _myConnection.Open();

                string command = "insert into clientsdailydocs (created, modified, createuser_id, lastuser_id, client_id, for_day, content_bodily, content_psychic, content_external_contact, content_responsibilities, draft, insert_key, content_school) values ('" + clientsdailydoc.created.ToString("yyyy-MM-dd HH:mm") + "', '" + clientsdailydoc.modified.ToString("yyyy-MM-dd HH:mm") + "', " + clientsdailydoc.createuser_id + ", " + clientsdailydoc.lastuser_id + ", '" + clientsdailydoc.for_day.ToString("yyyy-MM-dd HH:mm") + "', '" + clientsdailydoc.content_bodily + "', '" + clientsdailydoc.content_psychic + "', '" + clientsdailydoc.content_external_contact + "', '" + clientsdailydoc.content_responsibilities + "', " + clientsdailydoc.draft + ", " + clientsdailydoc.insert_key + ", " + clientsdailydoc.content_school + ");";
                MySqlCommand myTg = new MySqlCommand(command, _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch 
            {
                /**/
                    /**/
            }
            finally
            {
                _myConnection.Close();
            }
        }

        
    }
}
