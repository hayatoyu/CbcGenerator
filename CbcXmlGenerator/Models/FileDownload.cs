using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using log4net;

namespace CbcXmlGenerator.Models
{
    public class FileDownload
    {
        ILog log = log4net.LogManager.GetLogger(typeof(FileDownload));

        public string Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileStream { get; set; }
        public DateTime CreateDate { get; set; }

        private static string INSERT_FILE = "INSERT INTO FileDownload (Id,FileName,FileStream,CreateDate) VALUES (@Id,@FileName,@FileStream,@CreateDate)";
        public FileDownload Insert(FileDownload file)
        {
            FileDownload f = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(Commons.connStr))
                {
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("@Id", file.Id, System.Data.DbType.String);
                    dp.Add("@FileName", file.FileName, DbType.String);
                    dp.Add("@FileStream", file.FileStream, DbType.Binary);
                    dp.Add("@CreateDate", file.CreateDate, DbType.DateTime);
                    conn.Execute(INSERT_FILE, dp);
                    log.DebugFormat("INSERT {0} to DB Successfully", file.Id);
                }
                f = GetFile(file.Id);
            }
            catch(Exception e)
            {
                log.DebugFormat("ErrorType : {0},Message : {1},StackTrace : {2}", e.GetType(), e.Message, e.StackTrace);
            }
            return f;
        }

        private static string SELECT_FILE = "SELECT * FROM FileDownload WHERE Id = @Id";
        public FileDownload GetFile(string id)
        {
            FileDownload file = null;
            try
            {                
                using (SqlConnection conn = new SqlConnection(Commons.connStr))
                {
                    file = conn.Query<FileDownload>(SELECT_FILE, new { Id = id }).FirstOrDefault();
                }
            }
            catch(Exception e)
            {
                log.DebugFormat("ErrorType : {0},Message : {1},StackTrace : {2}", e.GetType(), e.Message, e.StackTrace);
            }
            return file;
        }

        private static string DELETE_FILE = "DELETE FROM FileDownload WHERE Id = @Id";
        public void DeleteFile(string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Commons.connStr))
                {
                    conn.Execute(DELETE_FILE, new { Id = id });
                }
                log.DebugFormat("DELETE {0} From DB Successfully", id);
            }
            catch(Exception e)
            {
                log.DebugFormat("ErrorType : {0},Message : {1},StackTrace : {2}", e.GetType(), e.Message, e.StackTrace);
            }
        }
    }
}