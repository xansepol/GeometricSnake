using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometricSnake.Application.DB
{
    internal class Connection
    {
        private SQLiteConnection _connection;

        public Connection()
        {
            _connection = new SQLiteConnection($"URI=file:{AppDomain.CurrentDomain.BaseDirectory}GeometricSnake.db");
        }

        public int RunCommand(string command)
        {
            SQLiteCommand cmd = new SQLiteCommand(command, _connection);
            return cmd.ExecuteNonQuery();
        }

        public SQLiteDataReader SelectCommand(string command)
        {
            SQLiteCommand cmd = new SQLiteCommand(command, _connection);
            return cmd.ExecuteReader();
        }

        public void Open()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
                _connection.Open();
        }

        public void Close()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                _connection.Close();
        }
    }
}
