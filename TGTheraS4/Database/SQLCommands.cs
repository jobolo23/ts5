﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DataModels;
using LinqToDB;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using TheraS5.Database.Objects;
using TheraS5.Objects;
using Document = TheraS5.Objects.Document;
using Instruction = TheraS5.Objects.Instruction;
using Service = TheraS5.Objects.Service;
using Shout = TGTheraS4.Objects.Shout;
using Taschengeld = TheraS5.Objects.Taschengeld;
using Task = TheraS5.Objects.Task;
using Title = IntranetTG.Objects.Title;
using User = IntranetTG.Objects.User;

namespace IntranetTG {
    public class SQLCommands
    {
        public enum ClientFilter
        {
            NotLeft,
            Left,
            All
        }

        private readonly MySqlConnection _myConnection;

        public SQLCommands(MySqlConnectionInformation connectionData)
        {
            _myConnection =
                new MySqlConnection(
                    $"uid={connectionData.Username};pwd={connectionData.Password};server={connectionData.Host};database={connectionData.Database}; Convert Zero Datetime=True");
        }

        public List<NewestDokus> getDokusByWgsAndDate(string[] wgs, string dateFrom)
        {
            var list = new List<NewestDokus>();
            var sql_wgs = "";
            for (var i = 0; i < wgs.Length; i++)
            {
                if (wgs.Length - i > 1)
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
                var myCommand =
                    new MySqlCommand(
                        "SELECT c.firstname cfirstname, c.lastname clastname, d.created dcreated, d.for_day, u.firstname ufirstname, u.lastname ulastname, s.name sname FROM clientsdailydocs d JOIN clients c ON c.id = d.client_id JOIN clientstoservices cs ON c.id = cs.client_id JOIN services s ON cs.service_id = s.id JOIN users u ON u.id = d.createuser_id WHERE (" +
                        sql_wgs + ") AND d.created > '" + dateFrom + "' ORDER BY d.created DESC", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var nn1 = "";
                    var nn2 = "";
                    nn1 = myReader["cfirstname"].ToString().Contains(" ") ? myReader["cfirstname"].ToString().Replace(' ', ',') : myReader["cfirstname"].ToString();
                    nn2 = myReader["clastname"].ToString().Contains(" ") ? myReader["clastname"].ToString().Replace(' ', ',') : myReader["clastname"].ToString();
                    var name = nn1 + " " + nn2;
                    var tag = myReader["for_day"].ToString().Split(' ')[0];
                    var created = myReader["dcreated"].ToString().Split(' ')[0];
                    var wg = myReader["sname"].ToString();
                    var ersteller = myReader["ufirstname"] + " " + myReader["ulastname"];
                    const string art = "Tagesdokumentation";
                    list.Add(new NewestDokus(name, tag, wg, ersteller, art, created,
                        tag + " - " + myReader["dcreated"]));
                }

                myReader.Close();


                myCommand = new MySqlCommand(
                    "SELECT c.firstname cfirstname, c.lastname clastname, r.created rcreated, r.name rname, r.art rart, u.firstname ufirstname, u.lastname ulastname, s.name sname FROM clientsreports r JOIN clients c ON c.id = r.client_id JOIN clientstoservices cs ON c.id = cs.client_id JOIN services s ON cs.service_id = s.id JOIN users u ON u.id = r.createuser_id WHERE (" +
                    sql_wgs + ") AND r.created > '" + dateFrom + "' ORDER BY r.created DESC", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var nn1 = "";
                    var nn2 = "";
                    nn1 = myReader["cfirstname"].ToString().Contains(" ") ? myReader["cfirstname"].ToString().Replace(' ', ',') : myReader["cfirstname"].ToString();
                    nn2 = myReader["clastname"].ToString().Contains(" ") ? myReader["clastname"].ToString().Replace(' ', ',') : myReader["clastname"].ToString();
                    var name = nn1 + " " + nn2;


                    var tag = myReader["rname"].ToString();
                    var created = myReader["rcreated"].ToString().Split(' ')[0];
                    var wg = myReader["sname"].ToString();
                    var ersteller = myReader["ufirstname"] + " " + myReader["ulastname"];
                    var art = "Bericht";
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
                    list.Add(new NewestDokus(name, tag, wg, ersteller, art, created,
                        tag + " - " + myReader["rcreated"]));
                }

                myReader.Close();


                myCommand = new MySqlCommand(
                    "SELECT c.firstname cfirstname, c.lastname clastname, r.created rcreated, r.name rname, r.art rart, u.firstname ufirstname, u.lastname ulastname, s.name sname FROM clientsfvgs r JOIN clients c ON c.id = r.client_id JOIN clientstoservices cs ON c.id = cs.client_id JOIN services s ON cs.service_id = s.id JOIN users u ON u.id = r.createuser_id WHERE (" +
                    sql_wgs + ") AND r.created > '" + dateFrom + "' ORDER BY r.created DESC", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var nn1 = "";
                    var nn2 = "";
                    nn1 = myReader["cfirstname"].ToString().Contains(" ") ? myReader["cfirstname"].ToString().Replace(' ', ',') : myReader["cfirstname"].ToString();
                    nn2 = myReader["clastname"].ToString().Contains(" ") ? myReader["clastname"].ToString().Replace(' ', ',') : myReader["clastname"].ToString();
                    var name = nn1 + " " + nn2;
                    var tag = myReader["rname"].ToString();
                    var created = myReader["rcreated"].ToString().Split(' ')[0];
                    var wg = myReader["sname"].ToString();
                    var ersteller = myReader["ufirstname"] + " " + myReader["ulastname"];
                    var art = "Bericht";
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
                    list.Add(new NewestDokus(name, tag, wg, ersteller, art, created,
                        tag + " - " + myReader["rcreated"]));
                }

                myReader.Close();


                list.Sort((o1, o2) => string.Compare(o1.created, o2.created, StringComparison.Ordinal));
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

        public Dictionary<string, string> WgToClients(string wg, ClientFilter filter)
        {
            using (var db = new Theras5DB())
            {
                var query = db.Clients.Join(db.Clientstoservices, c => c.Id, cs => cs.ClientId, (c, cs) => new {c, cs})
                    .Join(db.Services, t => t.cs.ServiceId, s => s.Id, (t, s) => new {t, s})
                    .Where(t => t.s.Name == wg && (filter == ClientFilter.NotLeft
                                    ? t.t.c.Leaving == null
                                    : (filter == ClientFilter.Left
                                        ? t.t.c.Leaving != null
                                        : t.t.c.Leaving == null || t.t.c.Leaving != null)))
                    .OrderBy(t => t.t.c.Lastname)
                    .ThenBy(t => t.s.Name)
                    .Select(t => t.t.c);

                return query.ToList().ToDictionary(client => client.Firstname, client => client.Lastname);
            }
        }

        public List<Document> GetClientdoku(int id, bool isPhoto)
        {
            using (var db = new Theras5DB())
            {
                return isPhoto
                    ? db.Clientsphotos.Where(cp => cp.Id == id).ToList().Select(doc => new Document(doc)).ToList()
                    : db.Clientsdocuments.Where(cd => cd.ClientId == id).ToList().Select(doc => new Document(doc))
                        .ToList();
            }
        }

        public List<WikiDoc> GetWikiDocs()
        {
            using (var db = new Theras5DB())
            {
                return db.Wikis.ToList().Select(wiki => new WikiDoc(wiki)).ToList();
            }
        }

        public List<Shout> GetShouts()
        {
            using (var db = new Theras5DB())
            {
                return db.Shouts.Join(db.Users, s => s.CreateuserId, u => u.Id, (s, u) => new {s, u})
                    .OrderBy(t => t.s.Created)
                    .Select(t => new Shout(t.s.Id, t.s.Created, t.s.CreateuserId, t.s.Message, t.u.Firstname, t.u.Lastname)).ToList();
            }
        }

        public void SetShout(string shout, string id)
        {
            using (var db = new Theras5DB())
            {
                db.Insert(new DataModels.Shout
                {
                    Created = DateTime.Now,
                    CreateuserId = Convert.ToUInt32(id),
                    Message = shout
                });
            }
        }

        internal string getDoku(string p, DateTime dateTime)
        {
            var ret = "";

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var temp = p.Split(' ');
                var myCommand = new MySqlCommand("select c.firstname,c.lastname, d.content_bodily, " +
                                                 "d.content_school, d.content_psychic, d.content_external_contact, d.content_responsibilities, d.createuser_id " +
                                                 "from clients c join clientsdailydocs d on c.id = d.client_id where c.firstname ='"
                                                 + temp[0].Replace(',', ' ') + "' and c.lastname ='" +
                                                 temp[1].Replace(',', ' ') + "' and d.for_day between'" +
                                                 dateTime.ToString("yyyy-MM-dd") + " 00:00' and '" +
                                                 dateTime.ToString("yyyy-MM-dd") + " 23:59'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["content_bodily"] + "$";
                    ret += myReader["content_school"] + "$";
                    ret += myReader["content_psychic"] + "$";
                    ret += myReader["content_external_contact"] + "$";
                    ret += myReader["content_responsibilities"] + "$";
                    ret += myReader["createuser_id"] + "$";
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

        internal string getMedicationConfirmations(string kid, DateTime dt)
        {
            /*var ret = "";
            var temp = kid.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand(
                    "select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night, cmc.morning mo,cmc.midday mi,cmc.evening ev,cmc.night ni, cmc.created crea, cm.cancelled cmca from clients c" +
                    " join clientsmedications cm on c.id = cm.client_id" +
                    " join medicaments m on cm.medicament_id = m.id" +
                    " join clientsmedicationsconfirmations cmc on cmc.clientsmedication_id = cm.id" +
                    " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" +
                    temp[1].Replace(',', ' ') + "' " +
                    "and cm.to >= '" + dt.ToString("yyyy-MM-dd") + "' and cmc.for_day ='" + dt.ToString("yyyy-MM-dd") +
                    "'", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    ret += myReader["mid"] + "$";
                    ret += myReader["name"] + "$";
                    ret += myReader["mo"] + "$";
                    ret += myReader["mi"] + "$";
                    ret += myReader["ev"] + "$";
                    ret += myReader["ni"] + "$";
                    ret += myReader["crea"] + "$";
                    ret += myReader["cmca"] + "$";
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
            }*/

            return "";
        }

        internal bool medicationIsConfirmed(string kid, DateTime dt)
        {
            /*var ret = false;
            var temp = kid.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select cmc.confirmed confi from clients c" +
                                                 " join clientsmedications cm on c.id = cm.client_id" +
                                                 " join medicaments m on cm.medicament_id = m.id" +
                                                 " join clientsmedicationsconfirmations cmc on cmc.clientsmedication_id = cm.id" +
                                                 " where c.firstname = '" + temp[0].Replace(',', ' ') +
                                                 "' and c.lastname = '" + temp[1].Replace(',', ' ') + "' " +
                                                 "and cm.to >= '" + dt.ToString("yyyy-MM-dd") + "' and cmc.for_day ='" +
                                                 dt.ToString("yyyy-MM-dd") + "'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["confi"].ToString() == "1")
                    {
                        ret = true;
                    }
                }
                myReader.Close();
                return ret;
            }
            catch
            {
                MessageBox.Show("Die Medikation konnte nicht bestätigt werden", "Fehler", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            finally
            {
                _myConnection.Close();
            }*/

            return true;
        }

        internal void CancelMediForClient(string cmid)
        {
            using (var db = new Theras5DB())
            {
                db.Clientsmedications
                    .Where(c => c.Id == Convert.ToUInt32(cmid))
                    .Set(c => c.Cancelled, DateTime.Now)
                    .Update();
            }
        }

        internal void DeleteMediForClient(string cmid)
        {
            using (var db = new Theras5DB())
            {
                db.Clientsmedications
                    .Where(c => c.Id == Convert.ToUInt32(cmid))
                    .Delete();
            }
        }

        internal void DeleteMediActionForClient(string client, string date, string art, string desc)
        {
            var id = getMediActionIDbyName(art);

            using (var db = new Theras5DB())
            {
                db.Clientsmedicalactions
                    .Where(c => c.ClientId == Convert.ToUInt32(client) && c.Realized == DateTime.Parse(date) && c.Statement == desc &&
                                c.MedicalactionId == Convert.ToUInt32(id))
                    .Delete();
            }
        }

        internal void AddNewMedi(string name, string dis, string uid)
        {
            using (var db = new Theras5DB())
            {
                db.Insert(new Medicament
                {
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    CreateuserId = Convert.ToUInt32(uid),
                    LastuserId = Convert.ToUInt32(uid),
                    Name = name,
                    Description = dis
                });
            }
        }

        internal bool CheckMediIfObsolete(string mid)
        {
            try
            {
                using (var db = new Theras5DB())
                {
                    db.Clientsmedications.Where(
                            c => c.Id == Convert.ToUInt32(mid) &&
                                 (c.Cancelled > DateTime.MinValue && c.Cancelled <= DateTime.Now ||
                                  c.To < DateTime.Now))
                        .ToList();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        internal void updateMedi(string medi_id, string mid, string luid, string from, string to, string mo, string mi,
            string ev, string ni)
        {
            using (var db = new Theras5DB())
            {
                db.Clientsmedications
                    .Where(c=>c.Id == Convert.ToUInt32(mid))
                    .Set(c=>c.Modified, DateTime.Now)
                    .Set(c => c.LastuserId, Convert.ToUInt32(luid))
                    .Set(c => c.From, DateTime.Parse(from))
                    .Set(c => c.To, DateTime.Parse(to))
                    .Set(c => c.Morning, Convert.ToSByte(mo))
                    .Set(c => c.Midday, Convert.ToSByte(mi))
                    .Set(c => c.Evening, Convert.ToSByte(ev))
                    .Set(c => c.Night, Convert.ToSByte(ni))
                    .Set(c => c.MedicamentId, Convert.ToUInt32(medi_id))
                    .Update();
            }
        }

        internal void AddFunctions(string name)
        {
            using (var db = new Theras5DB())
            {
                db.Insert(new Function
                {
                    Name = name
                });
            }
        }

        internal void EditFunctions(string name, string id)
        {
            using (var db = new Theras5DB())
            {
                db.Functions
                    .Where(f => f.ID == Convert.ToUInt32(id))
                    .Set(f => f.Name, name)
                    .Update();
            }
        }

        internal DateTime GetMediDateFrom(string mid)
        {
            using (var db = new Theras5DB())
            {
                return db.Clientsmedications.Where(c => c.Id == Convert.ToUInt32(mid)).ToList().First().From.Value;
            }
        }

        internal DateTime GetMediDateTo(string mid)
        {
            using (var db = new Theras5DB())
            {
                return db.Clientsmedications.Where(c => c.Id == Convert.ToUInt32(mid)).ToList().First().To.Value;
            }
        }


        internal void Deluserfunc(int id)
        {
            using (var db = new Theras5DB())
            {
                db.Userstofunctions
                    .Where(c => c.User == Convert.ToUInt32(id))
                    .Delete();
            }
        }

        internal void Insertuserfunc(int id, UserFunctions fu)
        {
            using (var db = new Theras5DB())
            {
                db.Insert(new Userstofunction
                {
                    User = id,
                    Function = Convert.ToInt32(fu.id)
                });
            }
        }

        internal void SaveUserfunc(int id, List<UserFunctions> fu)
        {
            Deluserfunc(id);
            foreach (var u in fu)
            {
                Insertuserfunc(id, u);
            }
        }

        internal List<UserFunctions> getUserFunctions()
        {
            using (var db = new Theras5DB())
            {
                return db.Functions.Select(f => f).ToList().Select(function => new UserFunctions
                    {
                        id = function.ID.ToString(),
                        Name = function.Name
                    })
                    .ToList();
            }
        }

        internal List<UserFunctions> getUserFunctions(int id)
        {
            using (var db = new Theras5DB())
            {
                return db.Functions.Join(db.Userstofunctions, f => f.ID, u => u.Function, (f, u) => new {f, u})
                    .Where(t => t.u.User == id)
                    .Select(t => t.f).ToList().Select(function => new UserFunctions
                    {
                        id = function.ID.ToString(),
                        Name = function.Name
                    })
                    .ToList();
            }
        }

        internal string getMedicationForClient(string kid, DateTime dt, bool ignoreDate, bool confirmed)
        {
            /*var ret = "";
            var temp = kid.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = null;
                if (!confirmed)
                {
                    if (!ignoreDate)
                    {
                        myCommand = new MySqlCommand(
                            "select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night from clients c" +
                            " join clientsmedications cm on c.id = cm.client_id" +
                            " join medicaments m on cm.medicament_id = m.id" +
                            " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" +
                            temp[1].Replace(',', ' ') + "' " +
                            "and cm.to >= '" + dt.ToString("yyyy-MM-dd") +
                            "' and (cm.cancelled = '0000-00-00' or cm.cancelled >= '" + dt.ToString("yyyy-MM-dd") +
                            "')", _myConnection);
                    }
                    else
                    {
                        myCommand = new MySqlCommand(
                            "select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night from clients c" +
                            " join clientsmedications cm on c.id = cm.client_id" +
                            " join medicaments m on cm.medicament_id = m.id" +
                            " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" +
                            temp[1].Replace(',', ' ') + "'", _myConnection);
                    }
                }
                else
                {
                    myCommand = new MySqlCommand(
                        "select cm.id cmid, m.id mid, c.firstname,c.lastname,m.name,cm.morning,cm.midday,cm.evening,cm.night, cmc.morning mo,cmc.midday mi,cmc.evening ev,cmc.night ni, cmc.created crea from clients c" +
                        " join clientsmedications cm on c.id = cm.client_id" +
                        " join medicaments m on cm.medicament_id = m.id" +
                        " join clientsmedicationsconfirmations cmc on cmc.clientsmedication_id = cm.id" +
                        " where c.firstname = '" + temp[0].Replace(',', ' ') + "' and c.lastname = '" +
                        temp[1].Replace(',', ' ') + "' " +
                        "and cm.to >= '" + dt.ToString("yyyy-MM-dd") +
                        "' and (cm.cancelled = '0000-00-00' or cm.cancelled >= '" + dt.ToString("yyyy-MM-dd") +
                        "') and cmc.for_day ='" + dt.ToString("yyyy-MM-dd") + "' and cmc.modified IS NULL",
                        _myConnection);
                }
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    ret += myReader["name"] + "$";
                    ret += myReader["morning"] + "$";
                    ret += myReader["midday"] + "$";
                    ret += myReader["evening"] + "$";
                    ret += myReader["night"] + "$";
                    ret += myReader["mid"] + "$";
                    ret += myReader["cmid"] + "$";
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
            }*/

            return "";
        }

        internal string[] readDokuOverTime(string p, DateTime von, DateTime bis)
        {
            var ret = new string[5];

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var temp = p.Split(' ');
                var myCommand = new MySqlCommand(
                    "select u.firstname first,u.lastname last, c.firstname,c.lastname,d.content_bodily, DATE_FORMAT(d.for_day,'%d.%m.%Y') AS forday, d.created, " +
                    "d.content_school, d.content_psychic, d.content_external_contact, d.content_responsibilities, d.createuser_id " +
                    "from clients c join clientsdailydocs d on c.id = d.client_id join users u on d.createuser_id=u.id where c.firstname ='"
                    + temp[0].Replace(',', ' ') + "' and c.lastname ='" + temp[1].Replace(',', ' ') +
                    "' and d.for_day between'" + von.ToString("yyyy-MM-dd") + " 00:00' and '" +
                    bis.ToString("yyyy-MM-dd") + " 23:59'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret[0] += "Autor: " + myReader["first"] + " " + myReader["last"] + "\r\n" + " Klient: " +
                              myReader["firstname"] + " " + myReader["lastname"] + "\r\n Für den Tag: " +
                              myReader["forday"] + "\r\n Erstellt am: " + myReader["created"] + "\r\n" +
                              myReader["content_bodily"] + "\r\n\r\n";
                    ret[1] += "Autor: " + myReader["first"] + " " + myReader["last"] + "\r\n" + " Klient: " +
                              myReader["firstname"] + " " + myReader["lastname"] + "\r\n Für den Tag: " +
                              myReader["forday"] + "\r\n Erstellt am: " + myReader["created"] + "\r\n" +
                              myReader["content_school"] + "\r\n\r\n";
                    ret[2] += "Autor: " + myReader["first"] + " " + myReader["last"] + "\r\n" + " Klient: " +
                              myReader["firstname"] + " " + myReader["lastname"] + "\r\n Für den Tag: " +
                              myReader["forday"] + "\r\n Erstellt am: " + myReader["created"] + "\r\n" +
                              myReader["content_psychic"] + "\r\n\r\n";
                    ret[3] += "Autor: " + myReader["first"] + " " + myReader["last"] + "\r\n" + " Klient: " +
                              myReader["firstname"] + " " + myReader["lastname"] + "\r\n Für den Tag: " +
                              myReader["forday"] + "\r\n Erstellt am: " + myReader["created"] + "\r\n" +
                              myReader["content_external_contact"] + "\r\n\r\n";
                    ret[4] += "Autor: " + myReader["first"] + " " + myReader["last"] + "\r\n" + " Klient: " +
                              myReader["firstname"] + " " + myReader["lastname"] + "\r\n Für den Tag: " +
                              myReader["forday"] + "\r\n Erstellt am: " + myReader["created"] + "\r\n" +
                              myReader["content_responsibilities"] + "\r\n\r\n";
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
            var ret = new string[1];

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var temp = p.Split(' ');
                var myCommand = new MySqlCommand(
                    "select u.firstname first,u.lastname last, c.firstname,c.lastname,d.content_bodily, DATE_FORMAT(d.for_day,'%d.%m.%Y') AS forday, d.created, " +
                    "d.content_school, d.content_psychic, d.content_external_contact, d.content_responsibilities, d.createuser_id " +
                    "from clients c join clientsdailydocs d on c.id = d.client_id join users u on d.createuser_id=u.id where c.firstname ='"
                    + temp[0].Replace(',', ' ') + "' and c.lastname ='" + temp[1].Replace(',', ' ') +
                    "' and d.for_day between'" + von.ToString("yyyy-MM-dd") + " 00:00' and '" +
                    bis.ToString("yyyy-MM-dd") + " 23:59'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["content_bodily"].ToString() != "")
                    {
                        ret[0] += "Dokument: Körperlich " + "\r\n" + "Autor: " + myReader["first"] + " " +
                                  myReader["last"] + "\r\n" + " Klient: " + myReader["firstname"] + " " +
                                  myReader["lastname"] + "\r\n Für den Tag: " + myReader["forday"] +
                                  "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_bodily"] +
                                  "$";
                    }
                    if (myReader["content_school"].ToString() != "")
                    {
                        ret[0] += "Dokument: Schulisch " + "\r\n" + "Autor: " + myReader["first"] + " " +
                                  myReader["last"] + "\r\n" + " Klient: " + myReader["firstname"] + " " +
                                  myReader["lastname"] + "\r\n Für den Tag: " + myReader["forday"] +
                                  "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_school"] +
                                  "$";
                    }
                    if (myReader["content_psychic"].ToString() != "")
                    {
                        ret[0] += "Dokument: Psychisch " + "\r\n" + "Autor: " + myReader["first"] + " " +
                                  myReader["last"] + "\r\n" + " Klient: " + myReader["firstname"] + " " +
                                  myReader["lastname"] + "\r\n Für den Tag: " + myReader["forday"] +
                                  "\r\n Erstellt am: " + myReader["created"] + "\r\n" + myReader["content_psychic"] +
                                  "$";
                    }
                    if (myReader["content_external_contact"].ToString() != "")
                    {
                        ret[0] += "Dokument: Außenkontakt " + "\r\n" + "Autor: " + myReader["first"] + " " +
                                  myReader["last"] + "\r\n" + " Klient: " + myReader["firstname"] + " " +
                                  myReader["lastname"] + "\r\n Für den Tag: " + myReader["forday"] +
                                  "\r\n Erstellt am: " + myReader["created"] + "\r\n" +
                                  myReader["content_external_contact"] + "$";
                    }
                    if (myReader["content_responsibilities"].ToString() != "")
                    {
                        ret[0] += "Dokument: Pflichten " + "\r\n" + "Autor: " + myReader["first"] + " " +
                                  myReader["last"] + "\r\n" + " Klient: " + myReader["firstname"] + " " +
                                  myReader["lastname"] + "\r\n Für den Tag: " + myReader["forday"] +
                                  "\r\n Erstellt am: " + myReader["created"] + "\r\n" +
                                  myReader["content_responsibilities"] + "$";
                    }
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

        internal void setDoku(string kid, DateTime when, string koerperlich, string schulisch, string psychisch,
            string pflichten, string außen, string id)
        {
            try
            {
                var name = kid.Split(' ');
                _myConnection.Open();
                var clientId = "";

                var myCommand = new MySqlCommand("select id from clients where firstname ='"
                                                 + name[0].Replace(',', ' ') + "' and lastname ='" +
                                                 name[1].Replace(',', ' ') + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    clientId = myReader["id"].ToString();
                }


                _myConnection.Close();
                _myConnection.Open();


                myCommand = new MySqlCommand(
                    "insert into clientsdailydocs (created, createuser_id, client_id, for_day,content_bodily, content_psychic, content_external_contact ,content_responsibilities ,content_school) " +
                    "values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "'," + id + ",'" + clientId + "','" +
                    when.ToString("yyyy-MM-dd") + "','" + koerperlich.Replace("'", "`") + "','" +
                    psychisch.Replace("'", "`") + "','" + außen.Replace("'", "`") + "','" +
                    pflichten.Replace("'", "`") + "','" + schulisch.Replace("'", "`") + "')", _myConnection);
                var test = myCommand.ExecuteNonQuery();
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

        public string getNameByID(string id, bool lastFirst)
        {
            using (var db = new Theras5DB())
            {
                var query = db.Users.Where(u => u.Id == Convert.ToUInt32(id));

                return lastFirst
                    ? query.ToList().First().Lastname + " " + query.ToList().First().Firstname
                    : query.ToList().First().Firstname + " " + query.ToList().First().Lastname;
            }
        }

        public string getExtern()
        {
            return getData("users", new[] {"firstname", "lastname", "username", "inclusion"},
                new[] {"firstname", "lastname", "username", "inclusion"},
                " where s.leaving IS NULL and s.inclusion is null order by s.lastname, s.firstname");
        }

        public string getHolidayRequests()
        {
            return getData("usersholidays",
                new[] {"u.firstname", "u.lastname", "s.date_from", "s.date_to", "s.admin_status"},
                new[] {"createuser_id", "date_from", "date_to", "admin_status"}, " order by s.date_from, s.date_to");
        }

        public string getClients()
        {
            return getData("clients",
                new[] {"s.firstname", "s.lastname", "s.inclusion", "s.date_of_birth", "s.status", "s.mc_phone_1"},
                new[] {"firstname", "lastname", "inclusion", "date_of_birth", "status", "mc_phone_1"},
                " where s.leaving IS NULL and s.status = 0  order by s.lastname, s.firstname");
        }

        public string getServices()
        {
            return getData("services", new[] {"s.id", "s.name"}, new[] {"id", "name"}, " order by s.name");
        }

        internal void setMediForDay(string p, DateTime when, Medicaments medi, string id, string why)
        {
            /*try
            {
                var name = p.Split(' ');
                _myConnection.Open();
                var clientId = "";

                var myCommand = new MySqlCommand("select id from clients where firstname ='"
                                                 + name[0].Replace(',', ' ') + "' and lastname ='" +
                                                 name[1].Replace(',', ' ') + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    clientId = myReader["id"].ToString();
                }


                _myConnection.Close();
                _myConnection.Open();


                myCommand = new MySqlCommand(
                    "insert into clientsmedicationsconfirmations (created, createuser_id, client_id, for_day,clientsmedication_id, morning, midday ,evening ,night,confirmed,reason_confirmed) " +
                    "values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "'," + id + ",'" + clientId + "','" +
                    when.ToString("yyyy-MM-dd") + "','" + medi.cmId + "','" + boolToInt(medi.morningConfirmed) + "','" +
                    boolToInt(medi.middayConfirmed) + "','" + boolToInt(medi.eveningConfirmed) + "','" +
                    boolToInt(medi.nightConfirmed) + "','" + "1" + "','" + why + "')", _myConnection);
                var test = myCommand.ExecuteNonQuery();
                myReader.Close();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally
            {
                _myConnection.Close();
            }*/
        }

        private int boolToInt(bool p)
        {
            if (p)
            {
                return 1;
            }
            return 0;
        }

        public void renameWiki(WikiDoc tmp, string title, User u)
        {
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "update wiki set lastuser_id = " + u.Id + " , title='" + title + "' where id = " + tmp.client_id +
                    " ;", _myConnection);

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
            using (var db = new Theras5DB())
            {
                var query = db.Users.Where(x => x.Id == Convert.ToUInt32(p));

                return query.ToList().First().PwThera;
            }
        }

        internal void setNewPW(string pw, string id)
        {
            try
            {
                _myConnection.Open();


                var myCommand = new MySqlCommand("Update users set pwThera = '" + pw + "' where id = " + id,
                    _myConnection);
                var test = myCommand.ExecuteNonQuery();
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "Select s.name from users u join userstoservices us on u.id = us.user_id join services s on us.service_id=s.id where u.id = " +
                        p, _myConnection);
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
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal bool isAdmin(string p)
        {

            using (var db = new Theras5DB())
            {
                var query = db.Users.Where(x => x.Id == Convert.ToUInt32(p));

                var isAdmin = bool.Parse(query.ToList().First().IsAdmin);
                return isAdmin;

            }
           
        }

        public void AddUserstoServices(int user_id, List<Service> service_id)
        {
            try
            {
                _myConnection.Open();
                var command = service_id.Aggregate("", (current, serv) => current + ("insert into userstoservices (user_id, service_id) values (" + user_id + ", " + serv.Id + " ); "));

                var myCommand = new MySqlCommand(command, _myConnection);
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "Select firstname, lastname, username, social_insurance_number, street, zip, city, phone_1, fax, bank, bank_code, bank_account_number, email_user, email_password, weeklyHours, weeklyDays, isAdmin, date_of_birth,inclusion,leaving from users where users.id = " +
                        p, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["firstname"] + "$";
                    ret += myReader["lastname"] + "$";
                    ret += myReader["username"] + "$";
                    ret += myReader["social_insurance_number"] + "$";
                    ret += myReader["street"] + "$";
                    ret += myReader["zip"] + "$";
                    ret += myReader["city"] + "$";
                    ret += myReader["phone_1"] + "$";
                    ret += myReader["fax"] + "$";
                    ret += myReader["bank"] + "$";
                    ret += myReader["bank_code"] + "$";
                    ret += myReader["bank_account_number"] + "$";
                    ret += myReader["email_user"] + "$";
                    ret += myReader["email_password"] + "$";
                    ret += myReader["weeklyHours"] + "$";
                    ret += myReader["weeklyDays"] + "$";
                    ret += myReader["isAdmin"] + "$";
                    ret += myReader["date_of_birth"] + "$";
                    ret += myReader["inclusion"] + "$";
                    ret += myReader["leaving"] + "$";
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

        internal void setNewUser(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8,
            string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16, string p17,
            string p18, string p19, string p20, DateTime p21)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "INSERT INTO users(bank_account_number, firstname , lastname, bank , bank_code , city , fax , email_password, email_address , email_user , street , social_insurance_number , phone_1 , username , weeklyHours , weeklyDays , zip , pwThera , createuser_id , lastuser_id , date_of_birth , isAdmin, inclusion) VALUES ('" +
                        p1 + "' , '" + p2 + "' ,'" + p3 + "' ,'" + p4 + "' ,'" + p5 + "' ,'" + p6 + "' ,'" + p7 +
                        "' ,'" + p8 + "' ,'" + p9 + "', '" + p9 + "' ,'" + p10 + "' ,'" + p11 + "' ,'" + p12 + "' ,'" +
                        p13 + "' ,'" + p14 + "' ,'" + p15 + "' ,'" + p16 + "' ,'" + p17 + "','" + p18 + "','" + p18 +
                        "','" + Convert.ToDateTime(p19).ToString("yyyy-MM-dd HH:mm") + "','" + p20 + "', '" +
                        p21.ToString("yyyy-MM-dd HH:mm") + "');", _myConnection);
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

        public void setEmailPass(string id, string emailpass)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand("update users set email_password='" + emailpass + "' where id = '" + id + "' ; ",
                        _myConnection);

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
                var myCommand = new MySqlCommand("update users set Dropboxpw='" + pass + "' where id = '" + id + "' ; ",
                    _myConnection);

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

        internal void updateUser(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8,
            string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16, string p17,
            string p18, string p19, int p20, string p21, string p22, string p23)
        {
            try
            {
                _myConnection.Open();


                var myCommand =
                    new MySqlCommand(
                        "update users set bank_account_number='" + p1 + "', firstname='" + p2 + "' , lastname='" + p3 +
                        "', bank='" + p4 + "' , bank_code='" + p5 + "' , city='" + p6 + "' , fax='" + p7 +
                        "' , email_password='" + p8 + "' , email_user='" + p9 + "' , street='" + p10 +
                        "' , social_insurance_number='" + p11 + "' , phone_1='" + p12 + "' , username='" + p13 +
                        "' , weeklyHours='" + p14 + "' , weeklyDays='" + p15 + "' , zip='" + p16 + "' , pwThera='" +
                        p17 + "' , lastuser_id='" + p18 + "', isAdmin='" + p21 + "' , date_of_birth='" + p19 +
                        "' , leaving='" + p22 + "' , inclusion='" + p19 + "' where id = '" + p20 + "' ; ",
                        _myConnection);

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

        public string getIdbyName(string name)
        {
            {
                try
                {
                    _myConnection.Open();
                    var id = "";
                    var ar = name.Split(' ');
                    var command =
                        new MySqlCommand(
                            "select id from users where firstname='" + ar[0] + "' and lastname='" + ar[1] +
                            "' and leaving is null;", _myConnection);


                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader["id"].ToString();
                    }
                    reader.Close();
                    return id;
                }
                catch (Exception e)
                {
                    return e.ToString();
                    ;
                }
                finally
                {
                    _myConnection.Close();
                }
            }
        }

        public string getIdbyNameClients(string kid)
        {
            {
                try
                {
                    var name = kid.Split(' ');
                    _myConnection.Open();
                    var id = "";
                    var myCommand = new MySqlCommand("select id from clients where firstname ='"
                                                     + name[0].Replace(',', ' ') + "' and lastname ='" +
                                                     name[1].Replace(',', ' ') + "'", _myConnection);
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
                    return e.ToString();
                    ;
                }
                finally
                {
                    _myConnection.Close();
                }
            }
        }


        internal void updateUserNoPw(string p1, string p2, string p3, string p4, string p5, string p6, string p7,
            string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16,
            string p17, string p18, string p19, int p20, DateTime p21, DateTime p22)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "update users set bank_account_number='" + p1 + "', firstname='" + p2 + "' , lastname='" + p3 +
                        "', bank='" + p4 + "' , bank_code='" + p5 + "' , city='" + p6 + "' , fax='" + p7 +
                        "' , email_password='" + p8 + "' , email_user='" + p9 + "' , street='" + p10 +
                        "' , social_insurance_number='" + p11 + "' , phone_1='" + p12 + "' , username='" + p13 +
                        "' , weeklyHours='" + p14 + "' , weeklyDays='" + p15 + "' , zip='" + p16 + "', lastuser_id='" +
                        p17 + "', isAdmin='" + p19 + "', inclusion='" +
                        Convert.ToDateTime(p22).ToString("yyyy-MM-dd HH:mm") + "', leaving='" +
                        Convert.ToDateTime(p21).ToString("yyyy-MM-dd HH:mm") + "' , date_of_birth='" + p18 +
                        "' where id = '" + p20 + "' ; ", _myConnection);

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

        internal void updateUserNoPw(string p1, string p2, string p3, string p4, string p5, string p6, string p7,
            string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16,
            string p17, string p18, string p19, int p20, DateTime p22)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "update users set bank_account_number='" + p1 + "', firstname='" + p2 + "' , lastname='" + p3 +
                        "', bank='" + p4 + "' , bank_code='" + p5 + "' , city='" + p6 + "' , fax='" + p7 +
                        "' , email_password='" + p8 + "' , email_user='" + p9 + "' , street='" + p10 +
                        "' , social_insurance_number='" + p11 + "' , phone_1='" + p12 + "' , username='" + p13 +
                        "' , weeklyHours='" + p14 + "' , weeklyDays='" + p15 + "' , zip='" + p16 + "', lastuser_id='" +
                        p17 + "', isAdmin='" + p19 + "', inclusion='" +
                        Convert.ToDateTime(p22).ToString("yyyy-MM-dd HH:mm") + "', date_of_birth='" + p18 +
                        "' where id = '" + p20 + "' ; ", _myConnection);

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

        public void updateUsertoService(int p, List<Service> p_2)
        {
            try
            {
                _myConnection.Open();

                var myCommand = new MySqlCommand("delete from userstoservices where user_id = " + p, _myConnection);
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

            using (var db = new Theras5DB())
            {
                var query = db.Users.Where(x => x.Id == Convert.ToUInt32(id));

                var workingDays = query.ToList().First().WeeklyDays.ToString();
                return workingDays;

            }
         
        }

        public void setTaschengeld(string tgKlient, string diff, char zeichen, string com, string name)
        {
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "insert into taschengeld(Client_ID, TG_before, TG_diff, TG_after, Comment, Name, Date) values(" +
                    tgKlient + ", " +
                    "(select pocket_money from clients where id = " + tgKlient + "), " +
                    diff + ", " +
                    "(select pocket_money from clients where id = " + tgKlient + ") " + zeichen + " " + diff + ", '" +
                    com + "' , '" +
                    name + "' , '" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')", _myConnection);
                myCommand.ExecuteNonQuery();

                myCommand = new MySqlCommand(
                    "update clients set pocket_money = pocket_money" + zeichen + diff + " where id = " + tgKlient,
                    _myConnection);
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
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand("select pocket_money from clients where id = " + id, _myConnection);
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
                var tglist = new List<Taschengeld>();
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select Name, Date, TG_before, TG_diff, TG_after, Comment from taschengeld where Client_ID = " +
                    id + " order by ID desc", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    var name = myTgReader["Name"].ToString();
                    var date = myTgReader["Date"].ToString();
                    var before = Convert.ToDouble(myTgReader["TG_before"].ToString());
                    var diff = myTgReader["TG_diff"].ToString();
                    var after = Convert.ToDouble(myTgReader["TG_after"].ToString());
                    var com = myTgReader["Comment"].ToString();
                    var nachher = after - before;
                    var in_out = nachher > -1;
                    tglist.Add(in_out
                        ? new Taschengeld(name, date, nachher.ToString(), "-", after.ToString(), com)
                        : new Taschengeld(name, date, "-", (nachher * -1).ToString(), after.ToString(), com));
                }

                return tglist;
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

        internal string getFVGs(string p1, string p2)
        {
            try
            {
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 +
                    "' and lastname='" + p2 + "') and art=3", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"] + "$";
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
                var datum = p2.Split(' ')[0].Split('.');
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select * from clientsfvgs c where day(c.from) = '" + datum[0] + "' and month(c.from) = '" +
                    datum[1] + "' and year(c.from) = '" + datum[2] +
                    "' and client_id = (select id from clients where firstname='" + name + "' and lastname='" + p +
                    "') ", _myConnection);
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
                var datum = p2.Split(' ')[0].Split('.');
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select * from clientsfvgs c where day(c.from) = '" + datum[0] + "' and month(c.from) = '" +
                    datum[1] + "' and year(c.from) = '" + datum[2] +
                    "' and client_id = (select id from clients where firstname='" + name + "' and lastname='" + p +
                    "') ", _myConnection);
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
                var command = "";
                if (ber.table == 1)
                {
                    command = "select content from clientsfvgs where id = " + ber.id;
                }
                else
                {
                    command = "select content from clientsreports where id = " + ber.id;
                }
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(command, _myConnection);
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
                var command = ""; //swag

                command =
                    "insert into clientsreports (created, modified, createuser_id, client_id, name, content, art) values ('" +
                    datetime + "', '" + datetime + "' , " + u.Id + ", " + ber.Client_id + ", '" + ber.name + "', '" +
                    ber.content + "', " + ber.art + " );";


                _myConnection.Open();
                var myTg = new MySqlCommand(command, _myConnection);

                myTg.ExecuteNonQuery();
            }
            catch
            {
              
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
                var command = ""; //swag

                command = "insert into vorlagen_bericht (name, content) values ('" + ber.name + "', '" + ber.content +
                          "');";


                _myConnection.Open();
                var myTg = new MySqlCommand(command, _myConnection);

                myTg.ExecuteNonQuery();
            }
            catch
            {
             
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
                var command = "";
                if (ber.table == 1)
                {
                    command = "update clientsfvgs set content = '" + ber.content + "', modified = '" +
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', lastuser_id = " + u.Id + " where id = " +
                              ber.id;
                }
                else
                {
                    command = "update clientsreports set content = '" + ber.content + "', modified = '" +
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', lastuser_id = " + u.Id + " where id = " +
                              ber.id;
                }

                _myConnection.Open();
                var myTg = new MySqlCommand(command, _myConnection);
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
                var command = "";
                if (ber.table == 1)
                {
                    command = "update clientsfvgs set art = " + ber.art + " where id = " + ber.id;
                }
                else
                {
                    command = "update clientsreports set art = " + ber.art + " where id = " + ber.id;
                }

                _myConnection.Open();
                var myTg = new MySqlCommand(command, _myConnection);
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
            var ret = new List<Klienten_Berichte>();
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand();

                myCommand = new MySqlCommand("Select id, name, content from vorlagen_bericht", _myConnection);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var tmp = new Klienten_Berichte
                    {
                        id = Convert.ToInt32(myReader["id"].ToString()),
                        name = myReader["name"].ToString(),
                        content = myReader["content"].ToString()
                    };

                    ret.Add(tmp);
                }

                myReader.Close();

                return ret;
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

        public void InsertFirstPMoney(string uid)
        {
            var id = new List<string>();
            var value = new List<string>();
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand("SELECT id, pocket_money FROM clients", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    id.Add(myReader["id"].ToString());
                    value.Add(myReader["pocket_money"].ToString());
                }
                myReader.Close();
                for (var i = 0; i < id.Count; i++)
                {
                    myCommand = new MySqlCommand(
                        "insert into pocket_money (ID, val, comment, eintrUserID, datum) values (\'" + id[i] +
                        "\', \'" + value[i] + "\', 'Anfangsbestand des Taschengeldes', " + uid + ", CURDATE())",
                        _myConnection);
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "Select von FROM workingtime WHERE usersid='" + user +
                        "' AND art = 'Urlaub' AND holidayverfied = 1 AND DAY(datetimefrom)=" + from.Day +
                        " AND MONTH(datetimefrom)=" + from.Month + " AND YEAR(datetimefrom)=" + from.Year +
                        " AND DAY(datetimeto)=" + to.Day + " AND MONTH(datetimeto)=" + to.Month +
                        " AND YEAR(datetimeto)=" + to.Year + " AND comment ='" + comment +
                        "' AND MINUTE(datetimefrom)=" + from.Minute + " AND HOUR(datetimefrom)=" + from.Hour +
                        " AND MINUTE(datetimeto)=" + to.Minute + " AND HOUR(datetimeto)=" + to.Hour, _myConnection);
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

        internal void updatePath(OpenFileDialog ofd)
        {
        }

        internal void addpath(string filename, int creater, int client, string titel, int size)
        {
            try
            {
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "insert into clientsdocuments (client_id, created, modified, createuser_id, title, path, filesize) values (" +
                    client + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', " + creater + ", '" + titel + "', '/data/clients/" +
                    client + "/documents/" + filename + "', " + size + ");", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
               
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setHouse(string ID, string name, string street, string zip, string city, string tel, string email,
            string homepage, string start)
        {
            try
            {
                var date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "insert into services (created, modified, createuser_id, lastuser_id, name, street, zip, city, phone_2, email_address, home_page, start) values('" +
                    date + "' , '" + date + "' , '" + ID + "' , '" + ID + "' , '" + name + "' , '" + street + "' , '" +
                    zip + "' , '" + city + "' , '" + tel + "' , '" + email + "' , '" + homepage + "' , '" + start +
                    "');", _myConnection);
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "SELECT User FROM `kilometergeld` WHERE YEAR(Zeitvon) = " + year + " AND MONTH(Zeitvon) = " +
                        month + " AND User = " + id + " ;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret = myReader["User"].ToString();
                }

                myReader.Close();

                return ret != "";
            }
            catch
            {
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
                var myTg = new MySqlCommand(
                    "insert into login (pcname, datum) values ('" + Environment.MachineName + "','" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "');", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
                
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
                var myTg = new MySqlCommand(
                    "insert into wiki (created, modified, createuser_id, title, path) values ('" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                    "', " + creater + ", '" + titel + "', '/data/wiki/" + filename + "');", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
                 
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void updatePath(Document doc, string name, int modified_id, int client_id, string titel, int size)
        {
            try
            {
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "update clientsdocuments set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                    "' , lastuser_id= " + modified_id + ", path = '/data/clients/" + client_id + "/documents/" + name +
                    "', title = '" + titel + "' , filesize = " + size + " where title= '" + doc.title +
                    "' and path = '" + doc.path + "' and createuser_id = " + doc.createuser_id + " and created = '" +
                    doc.created.ToString("yyyy-MM-dd HH:mm") + "' and client_id = " + doc.client_id + ";",
                    _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
                
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public string getKMGSumme(string id, string month, string year)
        {
            var ret = "";

            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "select SUM(Summe) as 'Summe' from kilometergeld where User=" + id + " and Month(Zeitvon)='" +
                        month + "' and Year(Zeitvon)='" + year + "'" + " and Month(Zeitbis)='" + month +
                        "' and Year(Zeitbis)='" + year + "'", _myConnection);
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

            return ret != "" ? ret : "0";
        }

        internal string getClientpics(int id)
        {
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "select client_id, created, modified, createuser_id, lastuser_id, title, path, filesize from clientsphotos where client_id = " +
                        id, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["client_id"] + "$";
                    ret += myReader["created"] + "$";
                    ret += myReader["modified"] + "$";
                    ret += myReader["createuser_id"] + "$";
                    ret += myReader["lastuser_id"] + "$";
                    ret += myReader["title"] + "$";
                    ret += myReader["path"] + "$";
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
                var myTg = new MySqlCommand(
                    "insert into clientsphotos (client_id, created, modified, createuser_id, title, path, filesize) values (" +
                    client + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', " + creater + ", '" + titel + "', '/data/clients/" +
                    client + "/documents/" + filename + "', " + size + ");", _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
               
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
                var myTg = new MySqlCommand(
                    "update clientsphotos set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                    "' , lastuser_id= " + modified_id + ", path = '/data/clients/" + client_id + "/documents/" + name +
                    "', title = '" + titel + "' , filesize = " + size + " where title= '" + doc.title +
                    "' and path = '" + doc.path + "' and createuser_id = " + doc.createuser_id + " and created = '" +
                    doc.created.ToString("yyyy-MM-dd HH:mm") + "' and client_id = " + doc.client_id + ";",
                    _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
               
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
                var myTg = new MySqlCommand(
                    "delete from clientsdocuments where title= '" + doc.title + "' and path = '" + doc.path +
                    "' and createuser_id = " + doc.createuser_id + " and created = '" +
                    doc.created.ToString("yyyy-MM-dd HH:mm:ss") + "' and client_id = " + doc.client_id + ";",
                    _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
                
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
                var myTg = new MySqlCommand(
                    "delete from clientsphotos where title= '" + doc.title + "' and path = '" + doc.path +
                    "' and createuser_id = " + doc.createuser_id + " and created = '" +
                    doc.created.ToString("yyyy-MM-dd HH:mm:ss") + "' and client_id = " + doc.client_id + ";",
                    _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {
                
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
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 +
                    "' and lastname='" + p2 + "') and art=0 ", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"] + "$";
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
                var ret = new List<Klienten_Berichte>();

                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select id, modified, name, created from clientsreports where client_id = (select id from clients where firstname='" +
                    firstname + "' and lastname='" + lastname + "') and art=" + art, _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    var tmp = new Klienten_Berichte
                    {
                        id = Convert.ToInt32(myTgReader["id"].ToString()),
                        name = myTgReader["name"] + " - " + myTgReader["created"],
                        table = 2
                    };
                    ret.Add(tmp);
                }

                return ret;
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

        internal List<Klienten_Berichte> getBericht_clientsfvgs(string firstname, string lastname, int art)
        {
            try
            {
                var ret = new List<Klienten_Berichte>();

                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select id, modified, name, created from clientsfvgs where client_id = (select id from clients where firstname='" +
                    firstname + "' and lastname='" + lastname + "') and art=" + art, _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    var tmp = new Klienten_Berichte
                    {
                        id = Convert.ToInt32(myTgReader["id"].ToString()),
                        name = myTgReader["name"] + " - " + myTgReader["created"],
                        table = 1
                    };
                    ret.Add(tmp);
                }

                return ret;
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

        internal string getVorfall(string p1, string p2)
        {
            try
            {
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 +
                    "' and lastname='" + p2 + "') and art=1", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"] + "$";
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
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 +
                    "' and lastname='" + p2 + "') and art=2", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"] + "$";
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
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 +
                    "' and lastname='" + p2 + "') and art=4", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"] + "$";
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
                var tg = "";
                _myConnection.Open();
                var myTg = new MySqlCommand(
                    "select * from clientsfvgs where client_id = (select id from clients where firstname='" + p1 +
                    "' and lastname='" + p2 + "') and art=5", _myConnection);
                MySqlDataReader myTgReader = null;
                myTgReader = myTg.ExecuteReader();

                while (myTgReader.Read())
                {
                    tg += myTgReader["from"] + "$";
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

        internal void setPad(string s1, string s2, string s3, string s4, string s5, string s6)
        {
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "insert into clientssanctions(client_id, created, modified, createuser_id, lastuser_id, sanction, statement, date_from, date_to) values(" +
                    s2 + " , '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:SS") + "' , '" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:SS") + "' , " + s1 + " , " + s1 + " , '" + s3 + "' , '" +
                    s4 + "' , '" + s5 + "' , '" + s6 + "')", _myConnection);
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

        internal List<PadMas> getPad(string id)
        {
            var list = new List<PadMas>();
            var list2 = new List<PadMas>();
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "select created, createuser_id, sanction, statement, date_from, date_to from clientssanctions where client_id=" +
                        id + " order by created desc;", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    var created = myReader["created"].ToString();
                    var from = myReader["createuser_id"].ToString();
                    var mas = myReader["sanction"].ToString();
                    var stat = myReader["statement"].ToString();
                    var datVon = myReader["date_from"].ToString();
                    var datBis = myReader["date_to"].ToString();
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

            foreach (var p in list)
            {
                p.from = getNameByID(p.from, false);
                list2.Add(p);
            }

            return list2;
        }

        internal string getServicesIdByUserId(int id)
        {
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("Select service_id from userstoservices where user_id = " + id,
                    _myConnection);
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
                return null;
            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal bool checkWikiName(string p)
        {
            using (var db = new Theras5DB())
            {
                var query = db.Wikis.Any(x => x.Title == p);
                return query;
            }
        }

        internal void updatewiki(string name, int UserId, WikiDoc doc)
        {
            try
            {
                _myConnection.Open();
                var command = "update wiki set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                              "' , path = '/data/wiki/" + name + "', lastuser_id = " + UserId + " where id = " +
                              doc.client_id + ";";
                var myTg = new MySqlCommand(command, _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                _myConnection.Close();
            }
        }

        internal void delwikiDoc(WikiDoc doc)
        {
            try
            {
                _myConnection.Open();

                var command = "delete from wiki where title = '" + doc.Name + "' and path = '" + doc.path +
                              "' and modified = '" + doc.Verändert.ToString("yyyy-MM-dd HH:mm") + "' and created = '" +
                              doc.Erstellt.ToString("yyyy-MM-dd HH:mm") + "';";
                var myTg = new MySqlCommand(command, _myConnection);
                myTg.ExecuteNonQuery();
            }
            catch
            {

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
                var isset = -1;

                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "select count(*) as 'count' from feedback where userId = " + uid + " and wikiId = " +
                        tmp.client_id + ";", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    isset = Convert.ToInt32(myReader["count"].ToString()) == 0 ? 0 : 1;
                }

                myReader.Close();
                return isset;
            }
            catch
            {
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
                    var myCommand = new MySqlCommand(
                        "update feedback set rate = " + rating + " where userId = " + uid + " and wikiId = " +
                        tmp.client_id + ";", _myConnection);

                    myCommand.ExecuteNonQuery();
                }
                catch
                {
                    return -1;
                }
                finally
                {
                    _myConnection.Close();
                }
            }
            else
            {
                try
                {
                    _myConnection.Open();
                    var myCommand = new MySqlCommand(
                        "insert into feedback values (" + uid + ", '', " + rating + ", " + tmp.client_id + ");",
                        _myConnection);

                    myCommand.ExecuteNonQuery();
                }
                catch
                {
                    return -1;
                }
                finally
                {
                    _myConnection.Close();
                }
            }

            var sum = -1;
            var count = -1;
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "select sum(rate) as 'rate', count(*) as 'count' from feedback where wikiId = " +
                        tmp.client_id + ";", _myConnection);
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
                return -1;
            }
            finally
            {
                _myConnection.Close();
            }
            if (sum <= 0) return -1;
            {
                if (count <= 0) return -1;
                double rate = sum / count;
                rate = Math.Round(rate, 2);
                try
                {
                    _myConnection.Open();
                    var myCommand = new MySqlCommand(
                        "update wiki set rate = " + rate + " where id = " + tmp.client_id + ";", _myConnection);

                    myCommand.ExecuteNonQuery();
                    return rate;
                }
                catch
                {
                    return -1;
                }
                finally
                {
                    _myConnection.Close();
                }
            }
        }

        public string getWorkingTime(string p, DateTime dateTime)
        {
            var ret = "";

            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var temp = p.Split(' ');
                var myCommand = new MySqlCommand("select c.firstname,c.lastname, d.content_bodily, " +
                                                 "d.content_school, d.content_psychic, d.content_external_contact, d.content_responsibilities, d.createuser_id " +
                                                 "from clients c join clientsdailydocs d on c.id = d.client_id where c.firstname ='"
                                                 + temp[0].Replace(',', ' ') + "' and c.lastname ='" +
                                                 temp[1].Replace(',', ' ') + "' and d.for_day between'" +
                                                 dateTime.ToString("yyyy-MM-dd") + " 00:00' and '" +
                                                 dateTime.ToString("yyyy-MM-dd") + " 23:59'", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["content_bodily"] + "$";
                    ret += myReader["content_school"] + "$";
                    ret += myReader["content_psychic"] + "$";
                    ret += myReader["content_external_contact"] + "$";
                    ret += myReader["content_responsibilities"] + "$";
                    ret += myReader["createuser_id"] + "$";
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
            var ret = "";
            var allColumnsString = "";

            var i = 0;
            foreach (var column in columnsDb)
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
                var selectCommand = "";
                var fullname = "";


                if (conditions != "")
                {
                    selectCommand = "select " + allColumnsString + " from " + database + " s " + conditions;
                }
                else
                {
                    selectCommand = "select " + allColumnsString + " from " + database + " s ";
                }

                var myCommand = new MySqlCommand(selectCommand, _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var colInt = 0;
                    foreach (var column in columns)
                    {
                        if (column == "firstname" || column == "lastname")
                        {
                            fullname += myReader[column] + " ";
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

        public void setData(string database, List<string> dataColumns, List<string> data)
        {
            var columns = new List<string> {"created", "createuser_id", "modified", "lastuser_id"};
            var datas = new List<string>
            {
                DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Functions.Functions.ActualUserFromList.Id,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Functions.Functions.ActualUserFromList.Id
            };
            var allColumnsString = "";
            var allData = "";

            columns.AddRange(dataColumns);
            datas.AddRange(data);

            var i = 0;
            foreach (var column in columns)
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
            foreach (var dat in datas)
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
                var myCommand =
                    new MySqlCommand(
                        "insert into " + database + " ( " + allColumnsString + " ) values( " + allData + " )",
                        _myConnection);
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

        public void setFullData(string database, List<string> dataColumns, List<string> data)
        {
            var columns = new List<string> {"created", "modified"};
            var allColumnsString = "";
            var allData = "";

            columns.AddRange(dataColumns);

            var i = 0;
            foreach (var column in columns)
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
            foreach (var dat in data)
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
                var myCommand =
                    new MySqlCommand(
                        "insert into " + database + " (" + allColumnsString + ") values('" +
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' , '" +
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' ," + allData + ")", _myConnection);
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

        public void updateData(string database, List<string> dataColumns, List<string> data, int id)
        {
            var columns = new List<string> {"modified", "lastuser_id"};
            var dataList = new List<string>
            {
                DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Functions.Functions.ActualUserFromList.Id
            };
            var updateString = "";

            columns.AddRange(dataColumns);
            dataList.AddRange(data);

            var i = 0;
            foreach (var column in columns)
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
                var myCommand = new MySqlCommand("UPDATE " + database + " SET " + updateString + " where id = " + id,
                    _myConnection);
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

        public string getEmployees()
        {
            return getData("users", new[] {"s.id", "s.firstname", "s.lastname", "s.username", "s.inclusion"},
                new[] {"id", "firstname", "lastname", "username", "inclusion"},
                " where s.leaving IS NULL and s.inclusion is not null order by s.lastname, s.firstname");
        }

        public void setEmployee(List<string> data)
        {
            var columns = new List<string> {"firstname", "lastname", "username", "inclusion"};
            setData("users", columns, data);
        }

        public void updateEmployee(List<string> data, int id)
        {
            updateData("users", new List<string> {"firstname", "lastname", "username", "inclusion"}, data, id);
        }

        public string getMedicaments()
        {
            return getData("medicaments", new[] {"s.id", "s.name"}, new[] {"id", "name"}, " order by s.name ");
        }

        public void setMedicament(List<string> data)
        {
            setData("medicaments", new List<string> {"name", "createuser_id", "lastuser_id", "description"}, data);
        }

        public void updateMedicaments(List<string> data, int id)
        {
            updateData("medicaments", new List<string> {"name", "createuser_id", "lastuser_id", "description"}, data,
                id);
        }

        public string getMedicamentsByClient(int id)
        {
            return getData("clientsmedications",
                new[] {"s.medicament_id", "s.from", "s.to", "s.morning", "s.midday", "s.evening", "s.night"},
                new[] {"medicament_id", "from", "to", "morning", "midday", "evening", "night"},
                " where s.client_id = " + id);
        }

        public void setMedicamentByClient(List<string> data)
        {
            var columns = new List<string> {"name", "createuser_id", "lastuser_id", "end"};
            setData("clientsmedications", columns, data);
        }

        public void addMedicamentForClient(string[] data)
        {
            _myConnection.Open();
            var myCommand =
                new MySqlCommand(
                    "INSERT INTO clientsmedications (`client_id`, `created`, `modified`, `createuser_id`, `lastuser_id`, `medicament_id`, `from`, `to`, `morning`, `midday`, `evening`, `night`) VALUES ('" +
                    data[0] + "', '" + data[1] + "', '" + data[2] + "', '" + data[3] + "', '" + data[4] + "', '" +
                    data[5] + "', '" + data[6] + "', '" + data[7] + "', '" + data[8] + "', '" + data[9] + "', '" +
                    data[10] + "', '" + data[11] + "');", _myConnection);
            myCommand.ExecuteNonQuery();
            _myConnection.Close();
        }

        public void updateMedicamentByClient(List<string> data, int id)
        {
            var columns = new List<string> {"medicament_id", "from", "to", "morning", "midday", "evening", "night"};
            updateData("clientsmedications", columns, data, id);
        }

        public string getProjects()
        {
            return getData("projects", new[] {"s.id", "s.name"}, new[] {"id", "name"}, " order by s.name");
        }

        public void setHourTypes(List<string> data)
        {
            var columns = new List<string> {"name"};
            setData("projects", columns, data);
        }

        public void updateHourTypes(List<string> data, int id)
        {
            var columns = new List<string> {"name"};
            updateData("projects", columns, data, id);
        }

        public string getUserID(string name)
        {


            var ret = "";
            string[] names;
            try
            {
                names = name.Split(' ');
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select u.id from users u where u.username='" + name + "'",
                    _myConnection);
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
            var ret = "";
            var fullname = name.Split(' ');
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "select u.id from users u where u.firstname='" + fullname[0] + "' AND u.lastname='" +
                        fullname[1] + "'", _myConnection);
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
            var us5 = new List<User>();

            try
            {
                _myConnection.Open();
                MySqlDataReader reader = null;
                var myCommand = new MySqlCommand("select id from users", _myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    var us = new User {Id = reader["id"].ToString()};

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
            var us5 = new List<User>();

            try
            {
                _myConnection.Open();
                MySqlDataReader reader = null;
                var myCommand = new MySqlCommand("select id from users where leaving is NULL order by lastname",
                    _myConnection);
                reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    var us = new User {Id = reader["id"].ToString()};

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

        public string getUrlaubstime(int user)
        {
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select users.holidays_open FROM users WHERE users.id = " + user,
                    _myConnection);
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
            const string ret = "";
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "DELETE FROM workingtime WHERE usersid='" + user + "' AND DAY(datetimefrom)=" + @from.Day +
                    " AND MONTH(datetimefrom)=" + @from.Month + " AND YEAR(datetimefrom)=" + @from.Year +
                    " AND DAY(datetimeto)=" + to.Day + " AND MONTH(datetimeto)=" + to.Month + " AND YEAR(datetimeto)=" +
                    to.Year + " AND comment ='" + comment + "' AND MINUTE(datetimefrom)=" + @from.Minute +
                    " AND HOUR(datetimefrom)=" + @from.Hour + " AND MINUTE(datetimeto)=" + to.Minute +
                    " AND HOUR(datetimeto)=" + to.Hour, _myConnection);
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select overtime from users where id=" + uid + ";", _myConnection);
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "select art, datetimefrom, datetimeto from workingtime where usersid=" + id +
                        " AND (DAYOFWEEK(datetimefrom) = 7 OR DATE(datetimefrom) in (select date_holiday from officialholidays)) AND YEAR(datetimefrom) = " +
                        dat.Year + " AND MONTH(datetimefrom) = " + dat.Month + " AND comment not like 'Krank%';",
                        _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["art"] + "$";
                    ret += myReader["datetimefrom"] + "$";
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand;
                if (user == 0)
                {
                    if (isadmin)
                    {
                        myCommand = new MySqlCommand(
                            "select users.firstname, users.lastname, workingtime.art, workingtime.datetimefrom, workingtime.datetimeto, workingtime.comment, workingtime.holidayverfied FROM workingtime JOIN users ON workingtime.usersid = users.id WHERE (usersid = usersid OR (workingtime.holidayverfied = false)) AND (MONTH(datetimefrom) = " +
                            date.Month + " OR (holidayverfied = false AND art = 'Urlaub')) AND (YEAR(datetimefrom) = " +
                            date.Year + " OR (holidayverfied = false AND art = 'Urlaub'));", _myConnection);
                    }
                    else
                    {
                        myCommand = new MySqlCommand(
                            "select users.firstname, users.lastname, workingtime.art, workingtime.datetimefrom, workingtime.datetimeto, workingtime.comment, workingtime.holidayverfied FROM workingtime JOIN users ON workingtime.usersid = users.id WHERE (usersid = usersid) AND (MONTH(datetimefrom) = " +
                            date.Month + " OR (holidayverfied = false AND art = 'Urlaub')) AND (YEAR(datetimefrom) = " +
                            date.Year + " OR (holidayverfied = false AND art = 'Urlaub'));", _myConnection);
                    }
                }
                else
                {
                    if (isadmin)
                    {
                        myCommand = new MySqlCommand(
                            "select users.firstname, users.lastname, workingtime.art, workingtime.datetimefrom, workingtime.datetimeto, workingtime.comment, workingtime.holidayverfied FROM workingtime JOIN users ON workingtime.usersid = users.id WHERE (usersid = " +
                            user + " OR (holidayverfied = false AND art = 'Urlaub')) AND (MONTH(datetimefrom) = " +
                            date.Month + " OR (holidayverfied = false AND art = 'Urlaub')) AND (YEAR(datetimefrom) = " +
                            date.Year + " OR (holidayverfied = false AND art = 'Urlaub'));", _myConnection);
                    }
                    else
                    {
                        myCommand = new MySqlCommand(
                            "select users.firstname, users.lastname, workingtime.art, workingtime.datetimefrom, workingtime.datetimeto, workingtime.comment, workingtime.holidayverfied FROM workingtime JOIN users ON workingtime.usersid = users.id WHERE (usersid = " +
                            user + ") AND (MONTH(datetimefrom) = " + date.Month +
                            " OR (holidayverfied = false AND art = 'Urlaub')) AND (YEAR(datetimefrom) = " + date.Year +
                            " OR (holidayverfied = false AND art = 'Urlaub'));", _myConnection);
                    }
                }
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["firstname"] + "$";
                    ret += myReader["lastname"] + "$";
                    ret += myReader["art"] + "$";
                    ret += myReader["datetimefrom"] + "$";
                    ret += myReader["datetimeto"] + "$";
                    ret += myReader["comment"] + "$";
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select users.weeklyhours FROM users WHERE users.id = " + id,
                    _myConnection);
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
                MessageBox.Show("Es is ein Fehler aufgetreten! Ich weiß leider nicht warum :(", ":(",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _myConnection.Close();
            }
            return 0;
        }

        public string getNonWorkingDays()
        {
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select date_holiday FROM officialholidays", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["date_holiday"] + "%";
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
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "select username, firstname, lastname, id FROM users WHERE leaving is NULL order by lastname, firstname",
                        _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret += myReader["username"] + "$";
                    ret += myReader["firstname"] + "$";
                    ret += myReader["lastname"] + "$";
                    ret += myReader["id"] + "%";
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
            var test = 0;
            if (exists(uid, art, datetimefrom, datetimeto, comment) == false)
            {
                try
                {
                    _myConnection.Open();
                    var myCommand =
                        new MySqlCommand(
                            "insert into workingtime (usersid, art, datetimefrom, datetimeto, comment, holidayverfied) values (" +
                            uid + ", '" + art + "', '" + datetimefrom.Year + "." + datetimefrom.Month + "." +
                            datetimefrom.Day + " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00" + "', '" +
                            datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " + datetimeto.Hour +
                            ":" + datetimeto.Minute + ":00" + "', '" + comment + "', 0);", _myConnection);
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
            return test != 0;
        }

        public bool setHolidayverfied(int uid, DateTime datetimefrom, DateTime datetimeto, int isverifed, int adminid)
        {
            var test = 0;
            try
            {
                _myConnection.Open();
                MySqlCommand myCommand;
                if (isverifed == -1)
                {
                    myCommand = new MySqlCommand(
                        "UPDATE workingtime SET holidayverfied= NULL, von= " + adminid + " WHERE usersid=" + uid +
                        " AND datetimefrom= '" + datetimefrom.Year + "." + datetimefrom.Month + "." + datetimefrom.Day +
                        " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00' AND datetimeto= '" +
                        datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " + datetimeto.Hour + ":" +
                        datetimeto.Minute + ":00" + "'", _myConnection);
                }
                else
                {
                    myCommand = new MySqlCommand(
                        "UPDATE workingtime SET holidayverfied=" + isverifed + " , von= " + adminid +
                        " WHERE usersid=" + uid + " AND datetimefrom= '" + datetimefrom.Year + "." +
                        datetimefrom.Month + "." + datetimefrom.Day + " " + datetimefrom.Hour + ":" +
                        datetimefrom.Minute + ":00' AND datetimeto= '" + datetimeto.Year + "." + datetimeto.Month +
                        "." + datetimeto.Day + " " + datetimeto.Hour + ":" + datetimeto.Minute + ":00" + "'",
                        _myConnection);
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
            return test != 0;
        }

        public bool exists(int uid, string art, DateTime datetimefrom, DateTime datetimeto, string comment)
        {
            var ret = "";
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "select COUNT(*) as 'anz' from workingtime where usersid = " + uid + " AND art = '" + art +
                        "' AND  datetimefrom = '" + datetimefrom.Year + "." + datetimefrom.Month + "." +
                        datetimefrom.Day + " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00" +
                        "' AND datetimeto = '" + datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " +
                        datetimeto.Hour + ":" + datetimeto.Minute + ":00" + "' AND comment = '" + comment +
                        "' AND holidayverfied is not NULL;", _myConnection);
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
            return ret != "0";
        }

        public bool setWorkingtime(int uid, string art, DateTime datetimefrom, DateTime datetimeto, string comment)
        {
            var test = 0;
            if (exists(uid, art, datetimefrom, datetimeto, comment) == false)
            {
                try
                {
                    _myConnection.Open();
                    var myCommand =
                        new MySqlCommand(
                            "insert into workingtime (usersid, art, datetimefrom, datetimeto, comment) values (" + uid +
                            ", '" + art + "', '" + datetimefrom.Year + "." + datetimefrom.Month + "." +
                            datetimefrom.Day + " " + datetimefrom.Hour + ":" + datetimefrom.Minute + ":00" + "', '" +
                            datetimeto.Year + "." + datetimeto.Month + "." + datetimeto.Day + " " + datetimeto.Hour +
                            ":" + datetimeto.Minute + ":00" + "', '" + comment + "');", _myConnection);
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
            return test != 0;
        }

        public string getWorkingTimeByUser(string id)
        {
            return "";
        }

        public string getWorkingTimeById(string userId, string id)
        {
            return "";
        }

        public void setWorkingTime(List<string> data)
        {
            setData("usersworkingtimes",
                new List<string> {"s.datetime_from", "s.datetime_to", "s.comment", "s.project", "s.type"}, data);
        }

        public void updateWorkingTime(List<string> data, int id)
        {
            updateData("usersworkingtimes",
                new List<string> {"s.datetime_from", "s.datetime_to", "s.comment", "s.project", "s.type"}, data, id);
        }

        public void setInstruction(string date, string title, string desc,
            string uid)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "INSERT INTO workinginstructions(title, describtion , startdate , uid) VALUES ('" + title +
                        "' , '" + desc + "' , '" + date + "'" + " , " + uid + ");", _myConnection);
                var instructionid = "";
                myCommand.ExecuteNonQuery();
                myCommand = new MySqlCommand("select max(iid) abc from workinginstructions", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    instructionid = myReader["abc"].ToString();
                }
                myReader.Close();
                myCommand = new MySqlCommand(
                    "insert into readinstructions(iid,pid,checked) select " + instructionid + " , id , 0 from users",
                    _myConnection);
                myCommand.ExecuteNonQuery();
                myCommand = new MySqlCommand(
                    "update readinstructions set checked=1 where pid=" + uid + " and iid=" + instructionid,
                    _myConnection);
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
                var list = new List<Instruction>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "Select i.startdate a, i.title b, i.describtion c, i.uid d, u.firstname e, u.lastname f from workinginstructions i, users u where i.uid = u.id;",
                        _myConnection);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var startdate = myReader["a"].ToString();
                    var title = myReader["b"].ToString();
                    var desc = myReader["c"].ToString();
                    var uid = myReader["e"] + " " + myReader["f"];
                    list.Add(new Instruction(title, desc, startdate, uid));
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

        public void setInstructionRead(string p, string titel, string date, string desc)
        {
            try
            {
                _myConnection.Open();
                var instructionid = "";
                var myCommand =
                    new MySqlCommand(
                        "select iid abc from workinginstructions where title='" + titel + "' and startdate='" + date +
                        "' and describtion='" + desc + "';", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    instructionid = myReader["abc"].ToString();
                }
                myReader.Close();
                myCommand = new MySqlCommand(
                    "update readinstructions set checked=1 where pid=" + p + " and iid=" + instructionid,
                    _myConnection);
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

        public List<ReadInstruction> getInstructionRead(string titel, string date, string desc)
        {
            try
            {
                _myConnection.Open();
                var list = new List<ReadInstruction>();
                var instructionid = "";
                var myCommand =
                    new MySqlCommand(
                        "select iid abc from workinginstructions where title='" + titel + "' and startdate='" + date +
                        "' and describtion='" + desc + "';", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    instructionid = myReader["abc"].ToString();
                }
                myReader.Close();
                myCommand = new MySqlCommand(
                    "select  u.firstname f, u.lastname l , ri.checked c from readinstructions ri join users u on ri.pid = u.id where iid=" +
                    instructionid + ";", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var firstname = myReader["f"].ToString();
                    var lastname = myReader["l"].ToString();
                    var check = myReader["c"].ToString();
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

        public List<Instruction> getUnreadInstruction(string id)
        {
            try
            {
                var list = new List<Instruction>();
                _myConnection.Open();
                var sl = new List<string>();
                var myCommand =
                    new MySqlCommand("select iid abc from readinstructions where pid=" + id + " and checked = 0",
                        _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var instructionid = myReader["abc"].ToString();
                    sl.Add(instructionid);
                }
                myReader.Close();
                foreach (var laufuid in sl)
                {
                    myCommand = new MySqlCommand(
                        "Select i.startdate a, i.title b, i.describtion c, i.uid d, u.firstname e, u.lastname f from workinginstructions i, users u where iid = " +
                        laufuid + " and i.uid = u.id;", _myConnection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        var startdate = myReader["a"].ToString();
                        var title = myReader["b"].ToString();
                        var desc = myReader["c"].ToString();
                        var uid = myReader["e"] + " " + myReader["f"];
                        list.Add(new Instruction(title, desc, startdate, uid));
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

        public void setTasks(string uid1, string uid2, string startdate, string enddate, string desc)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "insert into taskt (uid1, uid2, startdate, enddate, descr, status) values (" + uid1 + " , " +
                        uid2 + " , '" + startdate + "' , '" + enddate + "' , '" + desc + "' , " + 0 + ");",
                        _myConnection);
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

        public void changeModefrom0to1(string uid1, string uid2, string startdate, string enddate, string desc)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "update taskt set status=1 where uid1=" + uid1 + " and uid2=" + uid2 + " and startdate='" +
                        startdate + "' and enddate='" + enddate + "' and descr='" + desc + "';", _myConnection);
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

        public void changeModefrom1to2(string uid1, string uid2, string startdate, string enddate, string desc)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "update taskt set status=2 where uid1=" + uid1 + " and uid2=" + uid2 + " and startdate='" +
                        startdate + "' and enddate='" + enddate + "' and descr='" + desc + "';", _myConnection);
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

        public void changeEnddateTasks(string uid1, string uid2, string startdate, string enddate, string desc)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "update taskt set enddate='" + enddate + "'where uid1=" + uid1 + " and uid2=" + uid2 +
                        " and startdate='" + startdate + "' and descr='" + desc + "';", _myConnection);
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

        public void changeModefrom1to0(string uid1, string uid2, string startdate, string enddate, string desc)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "update taskt set status=0 where uid1=" + uid1 + " and uid2=" + uid2 + " and startdate='" +
                        startdate + "' and enddate='" + enddate + "' and descr='" + desc + "';", _myConnection);
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


        public List<Task> getTasksfromUser(string id)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "select uid1, uid2, startdate, enddate, descr from taskt where uid2=" + id + " and status=0;",
                        _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                var data = new List<string[]>();
                while (myReader.Read())
                {
                    data.Add(new[]
                    {
                        myReader["uid1"].ToString(), myReader["uid2"].ToString(), myReader["startdate"].ToString(),
                        myReader["enddate"].ToString(), myReader["descr"].ToString()
                    });
                }
                myReader.Close();
                _myConnection.Close();

                return data.Select(dat => new Task(getNameByID(dat[0], false), getNameByID(dat[1], false), dat[2],
                    dat[3], dat[4])).ToList();
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

        public List<Task> getTasksforUser(string id)
        {
            try
            {
                _myConnection.Open();
                var list = new List<Task>();
                var myCommand =
                    new MySqlCommand(
                        "select uid1, uid2, startdate, enddate, descr from taskt where uid1=" + id + " and status=1;",
                        _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                var data = new List<string[]>();
                while (myReader.Read())
                {
                    data.Add(new[]
                    {
                        myReader["uid1"].ToString(), myReader["uid2"].ToString(), myReader["startdate"].ToString(),
                        myReader["enddate"].ToString(), myReader["descr"].ToString()
                    });
                }
                myReader.Close();
                _myConnection.Close();

                return data.Select(dat => new Task(getNameByID(dat[0], false), getNameByID(dat[1], false), dat[2],
                    dat[3], dat[4])).ToList();
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

        public List<Task> getCreatedTasksforUser(string id)
        {
            try
            {
                _myConnection.Open();
                var list = new List<Task>();
                var myCommand =
                    new MySqlCommand(
                        "select uid1, uid2, startdate, enddate, descr from taskt where uid1=" + id + " and status=0;",
                        _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                var data = new List<string[]>();
                while (myReader.Read())
                {
                    data.Add(new[]
                    {
                        myReader["uid1"].ToString(), myReader["uid2"].ToString(), myReader["startdate"].ToString(),
                        myReader["enddate"].ToString(), myReader["descr"].ToString()
                    });
                }
                myReader.Close();
                _myConnection.Close();

                return data.Select(dat => new Task(getNameByID(dat[0], false), getNameByID(dat[1], false), dat[2],
                    dat[3], dat[4])).ToList();
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


        public List<Task> getUrgentTasksfromUser(string id)
        {
            try
            {
                _myConnection.Open();
                var list = new List<Task>();
                var myCommand =
                    new MySqlCommand(
                        "select uid1, uid2, startdate, enddate, descr from taskt where uid2=" + id +
                        " and (Datediff(enddate,CurDate())<=5  or CurDate() >= enddate)  and status=0;", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                var data = new List<string[]>();
                while (myReader.Read())
                {
                    data.Add(new[]
                    {
                        myReader["uid1"].ToString(), myReader["uid2"].ToString(), myReader["startdate"].ToString(),
                        myReader["enddate"].ToString(), myReader["descr"].ToString()
                    });
                }
                myReader.Close();
                _myConnection.Close();

                return data.Select(dat => new Task(getNameByID(dat[0], false), getNameByID(dat[1], false), dat[2],
                    dat[3], dat[4])).ToList();
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

        public string getTasksFromUser(string id)
        {
            return getData("tasks", new[] {"s.id, u.firstname", "u.lastname", "s.name", "s.end", "s.status"},
                new[] {"id", "firstname", "lastname", "name", "end", "status"},
                " join  users u on u.id = s.createuser_id where s.user_id = " + id +
                " and s.end is not null and s.end > NOW() order by s.end ASC");
        }

        public string getImportantTasksFromUser(string id)
        {
            return getData("tasks", new[] {"s.id, u.firstname", "u.lastname", "s.name", "s.end", "s.status"},
                new[] {"id", "firstname", "lastname", "name", "end", "status"},
                " join  users u on u.id = s.user_id where s.user_id = " + id + " and s.end between '" +
                DateTime.Now.ToString("yyyy-MM-dd") + " 00:00' and '" + DateTime.Now.AddDays(5).ToString("yyyy-MM-dd") +
                " 23:59' order by s.end, u.firstname DESC");
        }

        public string getTasksForUserById(string id)
        {
            return getData("tasks",
                new[]
                {
                    "s.id, u.firstname", "u.lastname", "s.name", "s.description", "s.project_id", "s.start", "s.end",
                    "s.lastuser_id", "s.status", "s.createuser_id"
                },
                new[]
                {
                    "id", "firstname", "lastname", "name", "description", "project_id", "start", "end", "lastuser_id",
                    "status", "createuser_id"
                }, "join users u on s.lastuser_id = u.id where s.id = " + id + " order by s.end, u.firstname");
        }

        public string getTasksForUser(string id)
        {
            return getData("tasks", new[] {"s.id, u.firstname", "u.lastname", "name", "s.end", "s.status"},
                new[] {"id", "firstname", "lastname", "name", "end", "status"},
                " join  users u on s.createuser_id = u.id where s.user_id = " + id +
                " and s.end is not null and s.end > NOW() order by s.end DESC");
        }

        public string getTasksFromUserById(string id)
        {
            return getData("tasks",
                new[]
                {
                    "s.id, u.firstname", "u.lastname", "s.name", "s.description", "s.project_id", "s.start", "s.end",
                    "s.user_id", "status", "s.createuser_id"
                },
                new[]
                {
                    "id", "firstname", "lastname", "name", "description", "project_id", "start", "end", "user_id",
                    "status", "createuser_id"
                }, "join users u on s.createuser_id = u.id where s.id = " + id + " order by s.end, u.firstname");
        }

        public void setTask(List<string> data)
        {
            setData("tasks",
                new List<string> {"user_id", "start", "end", "name", "description", "project_id", "status"}, data);
        }

        public void updateTask(List<string> data, int id)
        {
            updateData("tasks",
                new List<string> {"user_id", "start", "end", "name", "description", "project_id", "status"}, data, id);
        }

        public List<string> getVital(string id)
        {
            try
            {
                var values = new List<string>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "Select sex, firstname, lastname, inclusion, date_of_birth, citizenship, district_authority, insurance, leaving, icd, place_of_birth, social_insurance_number, co_insured, contact_id, status, assignment from clients where id=" +
                        id + ";", _myConnection);
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

        public void setContacts(string createuserid, string institution, string salutation_id, string title_id,
            string firstname, string lastname, string job_company, string job_department, string job_street,
            string job_zip, string job_country, string job_city, string job_phone_1, string job_email, string comment,
            string job_function)
        {
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "insert into contacts (created, createuser_id, institution, salutation_id, title_id, firstname, lastname, job_company, job_department, job_street, job_zip, job_country, job_city, job_phone_1, job_email, comment, job_function) values ('" +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' , " + createuserid.Trim() + " , '" +
                    institution.Trim() + "' , " + salutation_id + " , " + title_id + " , '" + firstname.Trim() +
                    "' , '" + lastname.Trim() + "' , '" + job_company.Trim() + "' , '" + job_department.Trim() +
                    "' , '" + job_street.Trim() + "' , '" + job_zip.Trim() + "' , '" + job_country.Trim() + "' , '" +
                    job_city.Trim() + "' , '" + job_phone_1.Trim() + "' , '" + job_email.Trim() + "' , '" +
                    comment.Trim() + "' , '" + job_function.Trim() + "');", _myConnection);
                 myCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten!\nBitte überprüfen Sie die Eingabe!",
                    "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<Salutations> getsalutations()
        {
            var ret = new List<Salutations>();
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("Select id, name, name_long from salutations;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ret.Add(new Salutations(myReader["id"].ToString(), myReader["name"].ToString(),
                        myReader["name_long"].ToString()));
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

        public void changeContacts(string cid, string id, string s1, string s2, string s3, string s4, string s5,
            string s6, string s7, string s8, string s9, string s10, string s11, string s12, string s13, string s14,
            string s15, string s16, string s17, string s18, string s19, string s20)
        {
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "update contacts set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                    "', lastuser_id = '" + id.Trim() + "', institution = '" + s1.Trim() +
                    "', salutation_id = (select id from salutations where name='" + s2.Trim() +
                    "'), title_id = (select id from titles where name='" + s3.Trim() + "'), firstname = '" + s4.Trim() +
                    "', lastname = '" + s5.Trim() + "', job_company = '" + s6.Trim() + "', job_department = '" +
                    s7.Trim() + "', job_street = '" + s8.Trim() + "', job_zip = '" + s9.Trim() + "', job_country = '" +
                    s10.Trim() + "', job_city = '" + s11.Trim() + "', job_phone_1 = '" + s12.Trim() +
                    "', job_phone_2 = '" + s13.Trim() + "', job_fax = '" + s14.Trim() + "', job_email = '" +
                    s15.Trim() + "', job_www = '" + s16.Trim() + "', job_skype = '" + s17.Trim() + "', comment = '" +
                    s18.Trim() + "', groups = (select id from groups where name='" + s19.Trim() +
                    "'), job_function = '" + s20.Trim() + "' where id = " + cid.Trim(), _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten!\nBitte überprüfen Sie die Eingabe!",
                    "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void changeContacts2(string moduserid, string institution, string salutation_id, string title_id,
            string firstname, string lastname, string job_company, string job_department, string job_street,
            string job_zip, string job_country, string job_city, string job_phone_1, string job_email, string comment,
            string job_function, string id)
        {
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "update contacts set modified = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                    "', lastuser_id = '" + moduserid.Trim() + "', institution = '" + institution +
                    "', salutation_id = " + salutation_id + ", title_id = " + title_id + ", firstname = '" +
                    firstname.Trim() + "', lastname = '" + lastname.Trim() + "', job_company = '" + job_company.Trim() +
                    "', job_department = '" + job_department.Trim() + "', job_street = '" + job_street.Trim() +
                    "', job_zip = '" + job_zip.Trim() + "', job_country = '" + job_country.Trim() + "', job_city = '" +
                    job_city.Trim() + "', job_phone_1 = '" + job_phone_1.Trim() + "', job_email = '" +
                    job_email.Trim() + "', comment = '" + comment.Trim() + "', job_function = '" + job_function.Trim() +
                    "' where id = " + id.Trim(), _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten!\nBitte überprüfen Sie die Eingabe!",
                    "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void deleteContacts(string id)
        {
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand("delete from contacts where id = '" + id + "';", _myConnection);
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
                var fillTitle = new List<string>();

                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select name from titles", _myConnection);
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
                var fillTitle = new List<Title>();

                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select id, name from titles order by id", _myConnection);
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    fillTitle.Add(new Title(myReader["id"].ToString(), myReader["name"].ToString()));
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
                var fillGroups = new List<string>();

                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("select name from groups", _myConnection);
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
            var list = new List<Contacts>();

            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "select c.id, s.name as salutation, t.name as title, firstname, lastname, institution, job_company, job_department, job_country, job_zip, job_city, job_street, job_phone_1, job_phone_2, job_fax, job_email, job_www, job_skype, comment, g.name as groups, job_function from contacts c left join salutations s on c.salutation_id=s.id left join titles t on c.title_id=t.id left join groups g on c.groups=g.id",
                        _myConnection);
                var myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    var id = myReader["id"].ToString();
                    var salutation = myReader["salutation"].ToString();
                    var title = myReader["title"].ToString();
                    var firstname = myReader["firstname"].ToString();
                    var lastname = myReader["lastname"].ToString();
                    var institution = myReader["institution"].ToString();
                    var company = myReader["job_company"].ToString();
                    var department = myReader["job_department"].ToString();
                    var country = myReader["job_country"].ToString();
                    var zip = myReader["job_zip"].ToString();
                    var city = myReader["job_city"].ToString();
                    var street = myReader["job_street"].ToString();
                    var phone1 = myReader["job_phone_1"].ToString();
                    var phone2 = myReader["job_phone_2"].ToString();
                    var fax = myReader["job_fax"].ToString();
                    var email = myReader["job_email"].ToString();
                    var www = myReader["job_www"].ToString();
                    var skype = myReader["job_skype"].ToString();
                    var comment = myReader["comment"].ToString();
                    var groups = myReader["groups"].ToString();
                    var function = myReader["job_function"].ToString();

                    list.Add(new Contacts(id, salutation, title, firstname, lastname, institution, company, department,
                        country, zip, city, street, phone1, phone2, fax, email, www, skype, comment, groups, function));
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
            var list = new List<Contacts>();

            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "select c.id, s.name as salutation, t.name as title, firstname, lastname, institution, job_company, job_department, job_country, job_zip, job_city, job_street, job_phone_1, job_phone_2, job_fax, job_email, job_www, job_skype, comment, g.name as groups, job_function from contacts c left join salutations s on c.salutation_id=s.id left join titles t on c.title_id=t.id left join groups g on c.groups=g.id where job_function LIKE  '%sozialarbeiter%'",
                        _myConnection);
                var myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    var id = myReader["id"].ToString();
                    var salutation = myReader["salutation"].ToString();
                    var title = myReader["title"].ToString();
                    var firstname = myReader["firstname"].ToString();
                    var lastname = myReader["lastname"].ToString();
                    var institution = myReader["institution"].ToString();
                    var company = myReader["job_company"].ToString();
                    var department = myReader["job_department"].ToString();
                    var country = myReader["job_country"].ToString();
                    var zip = myReader["job_zip"].ToString();
                    var city = myReader["job_city"].ToString();
                    var street = myReader["job_street"].ToString();
                    var phone1 = myReader["job_phone_1"].ToString();
                    var phone2 = myReader["job_phone_2"].ToString();
                    var fax = myReader["job_fax"].ToString();
                    var email = myReader["job_email"].ToString();
                    var www = myReader["job_www"].ToString();
                    var skype = myReader["job_skype"].ToString();
                    var comment = myReader["comment"].ToString();
                    var groups = myReader["groups"].ToString();
                    var function = myReader["job_function"].ToString();

                    list.Add(new Contacts(id, salutation, title, firstname, lastname, institution, company, department,
                        country, zip, city, street, phone1, phone2, fax, email, www, skype, comment, groups, function));
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

        public string getServiceIdbyClientId(string id)
        {
            try
            {
                var contacts = new List<Contacts>();
                var cid = "";
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("Select service_id from clientstoservices where client_id=" + id + ";",
                    _myConnection);
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
                var services = new List<Service>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("Select id , name from services;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var id = myReader["id"].ToString();
                    var name = myReader["name"].ToString();
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

        public bool setVital(string sex, string firstname, string lastname, string inclusion, string date_of_birth,
            string citizenship, string district_authority, string insurance, string icd, string place_of_birth,
            string social_insurance_number, string co_insured, string contact_id, string service_id, string status,
            string assignment)
        {
            try
            {
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "insert into clients(sex, firstname, lastname, inclusion, date_of_birth, citizenship, district_authority, insurance, icd, place_of_birth, social_insurance_number, co_insured, contact_id, status, assignment) values(" +
                        sex + ",'" + firstname + "','" + lastname + "','" + inclusion + "','" + date_of_birth + "','" +
                        citizenship + "','" + district_authority + "','" + insurance + "','" + icd + "','" +
                        place_of_birth + "','" + social_insurance_number + "','" + co_insured + "'," + contact_id +
                        ",'" + status + "','" + assignment + "');", _myConnection);
                myCommand.ExecuteNonQuery();
                _myConnection.Close();
                _myConnection.Open();
                var clientid = "";
                myCommand = new MySqlCommand("select max(id) abc from clients", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    clientid = myReader["abc"].ToString();
                }
                _myConnection.Close();
                _myConnection.Open();
                myCommand = new MySqlCommand(
                    "insert into clientstoservices(client_id,service_id) values(" + clientid + " , " + service_id +
                    ");", _myConnection);
                myCommand.ExecuteNonQuery();


                var check = clientid;

                if (check != null)
                {
                    var ftp = new FtpHandler();
                    ftp.CreatePathForClient(check);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                _myConnection.Close();
            }
            return true;
        }

        public void changeVital(string id, string sex, string firstname, string lastname, string inclusion,
            string date_of_birth, string citizenship, string district_authority, string insurance, string icd,
            string place_of_birth, string social_insurance_number, string co_insured, string contact_id,
            string service_id, string leaving, string status, string assignment)
        {
            try
            {
                _myConnection.Open();
                MySqlCommand myCommannd;
                if (string.IsNullOrEmpty(leaving.Trim()))
                {
                    myCommannd =
                        new MySqlCommand(
                            "update clients set sex=" + sex + " , firstname='" + firstname + "' , lastname='" +
                            lastname + "' , date_of_birth='" + date_of_birth + "' , inclusion='" + inclusion +
                            "' , citizenship='" + citizenship + "' , district_authority='" + district_authority +
                            "' , insurance='" + insurance + "' , icd='" + icd + "' , place_of_birth='" +
                            place_of_birth + "' , social_insurance_number='" + social_insurance_number +
                            "' , co_insured='" + co_insured + "' , contact_id=" + contact_id + " , status='" + status +
                            "' , assignment='" + assignment + "' where id=" + id + ";", _myConnection);
                }
                else
                {
                    myCommannd =
                        new MySqlCommand(
                            "update clients set sex=" + sex + " , firstname='" + firstname + "' , lastname='" +
                            lastname + "' , date_of_birth='" + date_of_birth + "' , inclusion='" + inclusion +
                            "' , citizenship='" + citizenship + "' , district_authority='" + district_authority +
                            "' , insurance='" + insurance + "' , icd='" + icd + "' , place_of_birth='" +
                            place_of_birth + "' , social_insurance_number='" + social_insurance_number +
                            "' , co_insured='" + co_insured + "' , contact_id=" + contact_id + " , leaving=" + leaving +
                            " , status='" + status + "' , assignment='" + assignment + "' where id=" + id + ";",
                            _myConnection);
                }

                myCommannd.ExecuteNonQuery();
                _myConnection.Close();
                _myConnection.Open();
                myCommannd =
                    new MySqlCommand(
                        "update clientstoservices set service_id=" + service_id + " where client_id=" + id + ";",
                        _myConnection);
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

        public List<BodyInfo> getBodyInfo(string id)
        {
            try
            {
                var infos = new List<BodyInfo>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "Select created, size, weight from clientsvitalstats where client_id=" + id +
                        " order by created desc ;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var date = myReader["created"].ToString();
                    var size = myReader["size"].ToString();
                    var weight = myReader["weight"].ToString();
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

        public void setBodyInfo(string id, string size, string weight)
        {
            try
            {
                _myConnection.Open();
                var myCommannd =
                    new MySqlCommand(
                        "insert into clientsvitalstats(client_id,size,weight,created) values(" + id + " , " + size +
                        " , " + weight + " , curDate() );", _myConnection);
                myCommannd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show(
                    "Es gab einen Feher beim eintragen der Werte überprüfen sie ihre Eingabe. \n Die Zahlen müssn in diesem Format angegeben werden 0.0 ",
                    "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public void setnewMedicalActionType(string type)
        {
            try
            {
                _myConnection.Open();
                var myCommannd = new MySqlCommand("insert into medicalactions(name) values('" + type + "');",
                    _myConnection);
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
                var infos = new List<Art>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("Select id,name from medicalactions;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var id = myReader["id"].ToString();
                    var name = myReader["name"].ToString();
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
                var id = "";
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand = new MySqlCommand("Select id from medicalactions where name = '" + name + "';",
                    _myConnection);
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

        public void WasSollDieseMethodeKoennen(string id, string cid, string date, string desc, string artid)
        {
            try
            {
                _myConnection.Open();
                var myCommannd =
                    new MySqlCommand(
                        "insert into clientsmedicalactions(client_id,createuser_id,created,realized,medicalaction_id,statement) values(" +
                        cid + " , " + id + " , curDate() , '" + date + "' , " + artid + " , '" + desc + "');",
                        _myConnection);
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

        public List<MediAkt> GetClientMed(string rudi)
        {
            try
            {
                var infos = new List<MediAkt>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "Select medicalactions.name a, clientsmedicalactions.realized b, clientsmedicalactions.statement c from clientsmedicalactions JOIN medicalactions ON medicalactions.id = clientsmedicalactions.medicalaction_id where client_id=" +
                        rudi + " order by realized desc;", _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var name = myReader["a"].ToString();
                    var date = myReader["b"].ToString();
                    var desc = myReader["c"].ToString();
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

        public List<MediAkt> GetClientMed_Month(string rudi, string month, string year)
        {
            try
            {
                var infos = new List<MediAkt>();
                _myConnection.Open();
                MySqlDataReader myReader = null;
                var myCommand =
                    new MySqlCommand(
                        "Select medicalactions.name a, clientsmedicalactions.realized b, clientsmedicalactions.statement c from clientsmedicalactions JOIN medicalactions ON medicalactions.id = clientsmedicalactions.medicalaction_id where client_id=" +
                        rudi + " and month(clientsmedicalactions.realized) = " + month +
                        " and year(clientsmedicalactions.realized) = " + year + " order by realized desc;",
                        _myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var name = myReader["a"].ToString();
                    var date = myReader["b"].ToString();
                    var desc = myReader["c"].ToString();
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

        public void setKilometerGeld(string uid, string Kennzeichen, string Ortvon, string Ortbis, string Zeitvon,
            string Zeitbis, string Summe, string km)
        {
            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "insert into kilometergeld (User,Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km) Values(" +
                        uid + " , '" + Kennzeichen + "' , '" + Ortvon + "' , '" + Ortbis + "' , '" + Zeitvon + "' , '" +
                        Zeitbis + "' , " + Summe + " , " + km + ");", _myConnection);
                myCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Die Zeit wurde in keinem passenden Format eingeben XX:XX", "Achtung",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                _myConnection.Close();
            }
        }

        public List<KmG> getKilometerGeld(string uid)
        {
            try
            {
                _myConnection.Open();
                var list = new List<KmG>();
                var myCommand =
                    new MySqlCommand(
                        "select User,Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km from kilometergeld where User=" +
                        uid, _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var user = myReader["User"].ToString();
                    var kennzeichen = myReader["Kennzeichen"].ToString();
                    var ortvon = myReader["Ortvon"].ToString();
                    var ortbis = myReader["Ortbis"].ToString();
                    var zeitvon = myReader["Zeitvon"].ToString();
                    var zeitbis = myReader["Zeitbis"].ToString();
                    var km = myReader["km"].ToString();
                    var Summe = myReader["Summe"].ToString();
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

        public List<KmG> getKilometerGeld_Month(string uid, string month, string year)
        {
            try
            {
                _myConnection.Open();
                var list = new List<KmG>();
                var myCommand =
                    new MySqlCommand(
                        "select User,Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km from kilometergeld where User=" +
                        uid + " AND MONTH(Zeitvon) = " + month + " AND YEAR(Zeitvon) = " + year, _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var user = myReader["User"].ToString();
                    var kennzeichen = myReader["Kennzeichen"].ToString();
                    var ortvon = myReader["Ortvon"].ToString();
                    var ortbis = myReader["Ortbis"].ToString();
                    var zeitvon = myReader["Zeitvon"].ToString();
                    var zeitbis = myReader["Zeitbis"].ToString();
                    var km = myReader["km"].ToString();
                    var Summe = myReader["Summe"].ToString();
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

        public List<KmG> getKilometerGeld(string uid, string month, string year)
        {
            try
            {
                _myConnection.Open();
                var list = new List<KmG>();
                var myCommand =
                    new MySqlCommand(
                        "select User,Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km from kilometergeld where User=" +
                        uid + " and Month(Zeitvon)='" + month + "' and Year(Zeitvon)='" + year + "'" +
                        " and Month(Zeitbis)='" + month + "' and Year(Zeitbis)='" + year + "'", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var user = myReader["User"].ToString();
                    var kennzeichen = myReader["Kennzeichen"].ToString();
                    var ortvon = myReader["Ortvon"].ToString();
                    var ortbis = myReader["Ortbis"].ToString();
                    var zeitvon = myReader["Zeitvon"].ToString();
                    var zeitbis = myReader["Zeitbis"].ToString();
                    var km = myReader["km"].ToString();
                    var Summe = myReader["Summe"].ToString();
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

        public List<Haus> getKBHaeuser()
        {
            var ret = new List<Haus>();
            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand("SELECT id, name FROM services", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    var id = myReader["id"].ToString();
                    var name = myReader["name"].ToString();
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
            var ret = new List<KontoNr>();

            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "SELECT ID, KontoNr, Beschr FROM KontoNr WHERE HID = " + hid + " ORDER BY KontoNr ASC",
                        _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    var id = myReader["ID"].ToString();
                    var knr = myReader["KontoNr"].ToString();
                    var desc = myReader["Beschr"].ToString();
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
            var ret = new List<KassaBuchNode>();

            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "SELECT Kassabuch.ID, BelNr, Datum, Bezeichn, Brutto, Steuers, Netto, MWST, KontoNr.KontoNr, users.firstname, users.lastname, services.name FROM Kassabuch JOIN users ON Kassabuch.EintrUserID = users.id JOIN services ON Kassabuch.Haus = services.id JOIN KontoNr ON Kassabuch.KontoNr = KontoNr.ID WHERE Haus = " +
                        hid + " AND Kassabuch.Datum >= '" + from + "' AND Kassabuch.Datum <= '" + to +
                        "' ORDER BY BelNr desc", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                string id;
                while (myReader.Read())
                {
                    id = myReader["ID"].ToString();
                    var belnr = myReader["BelNr"].ToString();
                    var datum = myReader["Datum"].ToString();
                    var beschr = myReader["Bezeichn"].ToString();
                    var brutto = myReader["Brutto"].ToString();
                    var steuers = myReader["Steuers"].ToString();
                    var netto = myReader["Netto"].ToString();
                    var mwst = myReader["MWST"].ToString();
                    var knr = myReader["KontoNr"].ToString();
                    var user = myReader["firstname"] + " " + myReader["lastname"];
                    var haus = myReader["name"].ToString();


                    if (belnr != "" && knr != "" && beschr != "" && brutto != "" && steuers != "" && netto != "" &&
                        mwst != "" && knr != "" && user != "" && haus != "")
                    {
                        ret.Add(new KassaBuchNode(id, belnr, DateTime.Parse(datum), beschr, brutto, steuers, netto,
                            mwst, knr, user, haus));
                    }
                }
                myReader.Close();
                foreach (var t in ret)
                {
                    id = t.id.ToString();
                    myCommand = new MySqlCommand(
                        "SELECT SUM(Brutto) kassst FROM Kassabuch WHERE ID <= " + id + " AND Haus = " + hid,
                        _myConnection);
                    myReader = myCommand.ExecuteReader();
                    string kassst;
                    if (myReader.Read() && (kassst = myReader["kassst"].ToString()) != "")
                    {
                        t.addKassst(kassst);
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

        public void addKBEintr(string belnr, string knr, string beschr, string brutto, string steuers, string netto,
            string mwst, string datum, string uid, string hid)
        {
            const string abst = ", ";
            const string hk = "\'";
            try
            {
                brutto = brutto.Replace(",", ".");
                netto = netto.Replace(",", ".");
                mwst = mwst.Replace(",", ".");
                _myConnection.Open();
                var myCommand = new MySqlCommand(
                    "INSERT INTO Kassabuch (BelNr, Datum, Bezeichn, Brutto, Steuers, Netto, MWST, KontoNr, EintrUserID, Haus) VALUES (" +
                    belnr + abst + hk + datum + hk + abst + hk + beschr + hk + abst + brutto + abst + steuers + abst +
                    netto + abst + mwst + abst + knr + abst + uid + abst + hid + ")", _myConnection);
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
                var myCommand =
                    new MySqlCommand(
                        "INSERT INTO KontoNr (KontoNr, Beschr, HID) VALUES (\'" + knr + "\', \'" + desc + "\', " + hid +
                        ")", _myConnection);
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
            var ret = 0;

            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "SELECT MAX(BelNr) as BelNr FROM Kassabuch WHERE Haus = " + hid +
                        " AND YEAR(Datum) = YEAR(CURDATE())", _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                myReader.Read();
                ret = myReader["BelNr"].ToString() == "" ? 0 : int.Parse(myReader["BelNr"].ToString());
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
                var myCommand =
                    new MySqlCommand("SELECT SUM(Brutto) Netto FROM Kassabuch WHERE haus = " + hid + " AND isKey = 0",
                        _myConnection);
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
                
            }
            finally
            {
                _myConnection.Close();
            }
            return ret;
        }

        public float getKBBalanceBeforeDate(DateTime d)
        {
            return 0;
        }

        public int getLastKBYear()
        {
            return 2000;
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
                var myCommand = new MySqlCommand("UPDATE Kassabuch SET " + col + " = " + val + " WHERE ID = " + id,
                    _myConnection);
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
            var ret = -1;

            try
            {
                _myConnection.Open();
                var myCommand = new MySqlCommand("SELECT ID FROM KontoNr WHERE KontoNr = " + knr + " AND HID = " + hid,
                    _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                string s;
                if (myReader.Read() && (s = myReader["id"].ToString()) != "")
                {
                    ret = int.Parse(s);
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
            var ret = -1;
            string fir, las;
            try
            {
                fir = name.Split(' ')[0];
                las = name.Split(' ')[1];
            }
            catch
            {
                return ret;
            }

            try
            {
                _myConnection.Open();
                var myCommand =
                    new MySqlCommand(
                        "SELECT ID FROM users WHERE firstname = \'" + fir + "\' AND lastname = \'" + las + "\'",
                        _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                string s;
                if (myReader.Read() && (s = myReader["id"].ToString()) != "")
                {
                    ret = int.Parse(s);
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
                var myCommand =
                    new MySqlCommand(
                        "SELECT SUM(Brutto) Netto FROM Kassabuch WHERE haus = " + hid + " AND Datum < '" + dat +
                        "' AND isKey = 0", _myConnection);
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
                var myCommand =
                    new MySqlCommand(
                        "SELECT 1 fggt FROM Kassabuch WHERE (SELECT MAX(Datum) FROM Kassabuch) >= '" + dat + "'",
                        _myConnection);
                MySqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                if (!myReader.Read() || myReader["fggt"].ToString() == string.Empty)
                {
                    ret = float.NaN;
                    return ret;
                }
                myReader.Close();
                myCommand = new MySqlCommand(
                    "SELECT SUM(Netto) Netto FROM Kassabuch WHERE haus = " + hid + " AND Datum < '" + dat +
                    "' AND isKey = 0", _myConnection);
                myReader = null;
                myReader = myCommand.ExecuteReader();

                if (myReader.Read() && myReader["Netto"].ToString() != string.Empty)
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
    }
}