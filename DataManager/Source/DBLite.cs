using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.SqlClient;

using System.Windows.Forms;

namespace zDBManager
{
    public class DBLite
    {
        public OdbcConnection OdbcCon;
        private OdbcDataReader Odbcdr;

        public  SqlConnection SqlCon;
        private SqlDataReader Sqldr;

        public OleDbConnection OleDbCon;
        private OleDbDataReader OleDbdr;
        public Exception ExError;
        private byte ConType;
        private string Path;
        private string Password;
        private string ConFormat;


        public static DBLite dbMu;
        public static DBLite dbMe;
        public static DBLite mdb;

        public DBLite(byte ConnectionType)
        {
            if (ConnectionType == 1)
            {
                OdbcCon = new OdbcConnection();
                ConType = 1;
            }
            else if (ConnectionType == 3)
            {
                SqlCon = new SqlConnection();
                ConType = 3;
            }
        }

        public DBLite(string path, string password)
        {
            Path = path;
            OleDbCon = new OleDbConnection();
            Password = password;
            ConType = 2;
        }

        public bool Connect()
        {
            try
            {
                ExError = new Exception();
                if (Password == "")
                {
                    ConFormat = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0; Data source={0}", Path);
                }
                else
                {
                    ConFormat = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0; Data source={0};Jet OLEDB:Database Password={1}", Path, Password);
                }
                OleDbCon.ConnectionString = ConFormat;
                OleDbCon.Open();
                OleDbCon.Close();
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return false;
            }
            finally
            {
                if (OleDbCon.State != ConnectionState.Closed)
                    OleDbCon.Close();
            }
            return true;
        }

        public bool Connect(string Server, string DataBase, string User, string Password)
        {
            try
            {
                ExError = new Exception();
                string ConFormat = "Data Source=" + Server + "; Network Library=DBMSSOCN; Initial Catalog=" + DataBase + ";User ID=" + User + ";Password=" + Password + ";";
                SqlCon.ConnectionString = ConFormat;
                SqlCon.Open();
                SqlCon.Close();
                return true;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return false;
            }
            finally
            {
                if (SqlCon.State != ConnectionState.Closed)
                    SqlCon.Close();
            }
        }

        public bool Connect(string DNS, string Login, string Password) //If is MySql DNS = DataBase
        {
            try
            {
                ExError = new Exception();
                string ConFormat = "DSN=" + DNS + ";UID=" + Login + ";PWD=" + Password + ";";
                OdbcCon.ConnectionString = ConFormat;
                OdbcCon.Open();
                OdbcCon.Close();
                return true;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return false;
            }
            finally
            {
                if (OdbcCon.State != ConnectionState.Closed)
                    OdbcCon.Close();
            }
        }

        public void Close()
        {
            switch (ConType)
            {
                case 1:
                    {
                        if (OdbcCon.State != ConnectionState.Closed)
                            OdbcCon.Close();
                        if(Odbcdr != null)
                            if (!Odbcdr.IsClosed)
                                Odbcdr.Close();
                    } break;
                case 2:
                    {
                        if (OleDbCon.State != ConnectionState.Closed)
                            OleDbCon.Close();

                        if (OleDbdr != null)
                            if (!OleDbdr.IsClosed)
                                OleDbdr.Close();
                    } break;
                case 3:
                    {
                        if (SqlCon.State != ConnectionState.Closed)
                            SqlCon.Close();

                        if (Sqldr != null)
                            if (!Sqldr.IsClosed)
                                Sqldr.Close();
                    } break;
            }
        }

        public bool Exec(string Query)
        {
            try
            {
                ExError = new Exception();
                LogWindow.SqlLog.LogAdd(Query);
                switch (ConType)
                {
                    case 1:
                        {
                            OdbcCon.Open();
                            OdbcCommand cmd = new OdbcCommand(Query, OdbcCon);
                            cmd.ExecuteNonQuery();
                        } break;
                    case 2:
                        {
                            OleDbCon.Open();
                            OleDbCommand cmd = new OleDbCommand(Query, OleDbCon);
                            cmd.ExecuteNonQuery();
                        } break;
                    case 3:
                        {
                            SqlCon.Open();
                            SqlCommand cmd = new SqlCommand(Query, SqlCon);
                            cmd.ExecuteNonQuery();
                        } break;
                }
                return true;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return false;
            }
        }

        public int ExecWithResult(string Query)
        {
            try
            {
                ExError = new Exception();
                LogWindow.SqlLog.LogAdd(Query);
                switch (ConType)
                {
                    case 1:
                        {
                            OdbcCon.Open();
                            OdbcCommand cmd = new OdbcCommand(Query, OdbcCon);
                            return (Int32)cmd.ExecuteScalar();
                        }
                    case 2:
                        {
                            OleDbCon.Open();
                            OleDbCommand cmd = new OleDbCommand(Query, OleDbCon);
                            return (Int32)cmd.ExecuteScalar();
                        }
                    case 3:
                        {
                            SqlCon.Open();
                            SqlCommand cmd = new SqlCommand(Query, SqlCon);
                            return (Int32)cmd.ExecuteScalar();
                        }
                }
                return Int32.MaxValue;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return Int32.MaxValue;
            }
        }

        public bool Read(string Query)
        {
            try
            {
                ExError = new Exception();
                LogWindow.SqlLog.LogAdd(Query);
                switch (ConType)
                {
                    case 1:
                        {
                            Odbcdr = default(OdbcDataReader);
                            OdbcCommand GetData = default(OdbcCommand);
                            GetData = new OdbcCommand(Query, OdbcCon);

                            OdbcCon.Open();
                            Odbcdr = GetData.ExecuteReader();
                        } break;
                    case 2:
                        {
                            OleDbdr = default(OleDbDataReader);
                            OleDbCommand GetData = default(OleDbCommand);
                            GetData = new OleDbCommand(Query, OleDbCon);

                            OleDbCon.Open();
                            OleDbdr = GetData.ExecuteReader();
                        } break;
                    case 3:
                        {
                            Sqldr = default(SqlDataReader);
                            SqlCommand GetData = default(SqlCommand);
                            GetData = new SqlCommand(Query, SqlCon);

                            SqlCon.Open();
                            Sqldr = GetData.ExecuteReader();
                        } break;
                }
                return true;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return false;
            }
        }

        public bool Fetch()
        {
            try
            {
                ExError = new Exception();
                switch (ConType)
                {
                    case 1:
                        {
                            if (Odbcdr != null)
                            {
                                return Odbcdr.Read();
                            }
                        } break;
                    case 2:
                        {
                            if (OleDbdr != null)
                            {
                                return OleDbdr.Read();
                            }
                        } break;
                    case 3:
                        {
                            if (Sqldr != null)
                            {
                                return Sqldr.Read();
                            }
                        } break;
                }
                return false;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return false;
            }
        }


        public string GetAsString(string Row)
        {
            try
            {
                ExError = new Exception();
                switch (ConType)
                {
                    case 1:
                        {
                            if (!Odbcdr.IsClosed)
                            {
                                for (int i = 0; i < Odbcdr.FieldCount; i++)
                                {
                                    if (Odbcdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return Odbcdr[i].ToString();
                                    }
                                }
                            }
                        } break;
                    case 2:
                        {
                            if (!OleDbdr.IsClosed)
                            {
                                for (int i = 0; i < OleDbdr.FieldCount; i++)
                                {
                                    if (OleDbdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return OleDbdr[i].ToString();
                                    }
                                }
                            }
                        } break;
                    case 3:
                        {
                            if (!Sqldr.IsClosed)
                            {
                                for (int i = 0; i < Sqldr.FieldCount; i++)
                                {
                                    if (Sqldr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return Sqldr[i].ToString();
                                    }
                                }
                            }
                        } break;
                }
                return null;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return null;
            }
        }


        public int GetAsInteger(string Row)
        {
            try
            {
                ExError = new Exception();
                switch (ConType)
                {
                    case 1:
                        {
                            if (!Odbcdr.IsClosed)
                            {
                                for (int i = 0; i < Odbcdr.FieldCount; i++)
                                {
                                    if (Odbcdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return Convert.ToInt32(Odbcdr[i]);
                                    }
                                }
                            }
                        } break;
                    case 2:
                        {
                            if (!OleDbdr.IsClosed)
                            {
                                for (int i = 0; i < OleDbdr.FieldCount; i++)
                                {
                                    if (OleDbdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return Convert.ToInt32(OleDbdr[i]);
                                    }
                                }
                            }
                        } break;
                    case 3:
                        {
                            if (!Sqldr.IsClosed)
                            {
                                for (int i = 0; i < Sqldr.FieldCount; i++)
                                {
                                    if (Sqldr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return Convert.ToInt32(Sqldr[i]);
                                    }
                                }
                            }
                        } break;
                }
                return 0;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return 0;
            }
        }

        public int GetAsInteger(int Pos)
        {
            try
            {
                ExError = new Exception();
                switch (ConType)
                {
                    case 1:
                        {
                            if (!Odbcdr.IsClosed)
                            {
                                return Convert.ToInt32(Odbcdr[Pos]);
                            }
                        } break;
                    case 2:
                        {
                            if (!OleDbdr.IsClosed)
                            {

                                return Convert.ToInt32(OleDbdr[Pos]);

                            }
                        } break;
                    case 3:
                        {
                            if (!Sqldr.IsClosed)
                            {

                                        return Convert.ToInt32(Sqldr[Pos]);
                            }
                        } break;
                }
                return 0;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return 0;
            }
        }

        public Int64 GetAsInteger64(string Row)
        {
            try
            {
                ExError = new Exception();
                switch (ConType)
                {
                    case 1:
                        {
                            if (!Odbcdr.IsClosed)
                            {
                                for (int i = 0; i < Odbcdr.FieldCount; i++)
                                {
                                    if (Odbcdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return Convert.ToInt64(Odbcdr[i]);
                                    }
                                }
                            }
                        } break;
                    case 2:
                        {
                            if (!OleDbdr.IsClosed)
                            {
                                for (int i = 0; i < OleDbdr.FieldCount; i++)
                                {
                                    if (OleDbdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return Convert.ToInt64(OleDbdr[i]);
                                    }
                                }
                            }
                        } break;
                    case 3:
                        {
                            if (!Sqldr.IsClosed)
                            {
                                for (int i = 0; i < Sqldr.FieldCount; i++)
                                {
                                    if (Sqldr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return Convert.ToInt64(Sqldr[i]);
                                    }
                                }
                            }
                        } break;
                }
                return 0;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return 0;
            }
        }

        public float GetAsFloat(string Row)
        {
            try
            {
                ExError = new Exception();
                switch (ConType)
                {
                    case 1:
                        {
                            if (!Odbcdr.IsClosed)
                            {
                                for (int i = 0; i < Odbcdr.FieldCount; i++)
                                {
                                    if (Odbcdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return float.Parse(Odbcdr[i].ToString());
                                    }
                                }
                            }
                        } break;
                    case 2:
                        {
                            if (!OleDbdr.IsClosed)
                            {
                                for (int i = 0; i < OleDbdr.FieldCount; i++)
                                {
                                    if (OleDbdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return float.Parse(OleDbdr[i].ToString());
                                    }
                                }
                            }
                        } break;
                    case 3:
                        {
                            if (!Sqldr.IsClosed)
                            {
                                for (int i = 0; i < Sqldr.FieldCount; i++)
                                {
                                    if (Sqldr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return float.Parse(Sqldr[i].ToString());
                                    }
                                }
                            }
                        } break;
                }
                return 0;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return 0;
            }
        }

        public byte[] GetAsBinary(string Row)
        {
            try
            {
                ExError = new Exception();
                switch (ConType)
                {
                    case 1:
                        {
                            if (!Odbcdr.IsClosed)
                            {
                                for (int i = 0; i < Odbcdr.FieldCount; i++)
                                {
                                    if (Odbcdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return (byte[])(Odbcdr[i]);
                                    }
                                }
                            }
                        } break;
                    case 2:
                        {
                            if (!OleDbdr.IsClosed)
                            {
                                for (int i = 0; i < OleDbdr.FieldCount; i++)
                                {
                                    if (OleDbdr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return (byte[])(OleDbdr[i]);
                                    }
                                }
                            }
                        } break;
                    case 3:
                        {
                            if (!Sqldr.IsClosed)
                            {
                                for (int i = 0; i < Sqldr.FieldCount; i++)
                                {
                                    if (Sqldr.GetName(i).ToUpper() == Row.ToUpper())
                                    {
                                        return (byte[])(Sqldr[i]);
                                    }
                                }
                            }
                        } break;
                }
                return null;
            }
            catch (Exception x)
            {
                ExError = x;
                LogWindow.SqlLog.LogAdd(ExError.Message);
                return null;
            }
        }
    }
}
