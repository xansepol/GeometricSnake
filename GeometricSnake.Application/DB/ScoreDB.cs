using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeometricSnake.Application.DB
{
    internal class ScoreDB
    {
        private static ScoreDB? instance = null;
        private Connection _con;
        public int Score { get; private set; }
        private ScoreDB()
        {
            _con = new Connection();
            Init();
        }
        
        public static ScoreDB Instance()
        {
            if (instance is null)
                instance = new ScoreDB();

            return instance;
        }

        public void SetScore(int score)
        {
            if(score > Score)
            {
                Score = score;
                SaveScore(score);
            }
        }

        private void Init()
        {
            try
            {
                _con.Open();
                _con.RunCommand($"CREATE TABLE IF NOT EXISTS score(ID INTEGER PRIMARY KEY, VALUE INTEGER);");
                Score = GetDBScore();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _con.Close();
            }
        }

        private int GetDBScore()
        {
            using (SQLiteDataReader reader = _con.SelectCommand($"SELECT value FROM score"))
            {
                if (reader.Read())
                    return reader.GetInt32(0);
            }
            return 0;
        }

        private void SaveScore(int score)
        {
            try
            {
                _con.Open();
                int scoreDb = GetDBScore();
                if (scoreDb == 0)
                    InsertScore(score);
                else
                    UpdateScore(score);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _con.Close();
            }
        }

        private void InsertScore(int score)
        {
            _con.RunCommand($"INSERT INTO score VALUES(1, {score})");
        }

        private void UpdateScore(int score)
        {
            _con.RunCommand($"UPDATE score SET VALUE={score} WHERE ID = 1");
        }
    }
}
