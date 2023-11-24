using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System.Data;
using DotNetEnv;
public class CrimeDatabase {
    private readonly string connectionString;
    public CrimeDatabase() {
        DotNetEnv.Env.Load();
        connectionString = Environment.GetEnvironmentVariable("DB_URL");
    }
    public CrimeDatabase(string connectionString) {
        this.connectionString = connectionString;
    }

    public int GetNumberOfRecords() {
        using (NpgsqlConnection conn = new NpgsqlConnection(this.connectionString)) {
            NpgsqlCommand exec = new NpgsqlCommand("SELECT COUNT(*) FROM tbl_analytics", conn);
            object result = exec.ExecuteScalar();
            if (result != null)
            {
                int rowCount = Convert.ToInt32(result);
                Console.WriteLine($"Number of rows: {rowCount}");
                return rowCount;
            } else {
                return 0;
            }
        }
    }
    public CrimePageAPI GetCrimeData(int page, int pageSize) {
        List<CrimeSchema> l = new List<CrimeSchema>();
        using (NpgsqlConnection conn = new NpgsqlConnection(this.connectionString)) {
            conn.Open();
            int offset = (page - 1) * pageSize;
            
            string stmt = "SELECT * FROM tbl_analytics ORDER BY crime_date DESC LIMIT @pageSize OFFSET @offset";
            NpgsqlCommand exec = new NpgsqlCommand(stmt, conn);
            exec.Parameters.AddWithValue("pageSize", pageSize);
            exec.Parameters.AddWithValue("offset", offset);

            NpgsqlDataReader results = exec.ExecuteReader();
            while(results.Read())
            {
                var crimeData = new CrimeSchema(
                    results.IsDBNull(0) ? "" : results.GetString(0),
                    results.GetDateTime(1),
                    results.IsDBNull(2) ? "" : results.GetString(2),
                    results.IsDBNull(3) ? -1.0f : results.GetFloat(3), 
                    results.IsDBNull(4) ? -1.0f : results.GetFloat(4), 
                    results.IsDBNull(5) ? "" : results.GetString(5),
                    results.IsDBNull(6) ? "" : results.GetString(6),
                    results.IsDBNull(7) ? "" : results.GetString(7),
                    results.IsDBNull(8) ? false : results.GetBoolean(8),
                    results.IsDBNull(9) ? "" : results.GetString(9),
                    results.IsDBNull(10) ? "" : results.GetString(10),
                    results.IsDBNull(11) ? false : results.GetBoolean(11)
                );
                l.Add(crimeData);
            }
        }
        int totalPage = 0;
        using (NpgsqlConnection conn = new NpgsqlConnection(this.connectionString)) {
            conn.Open();
            NpgsqlCommand exec = new NpgsqlCommand("SELECT COUNT(*) FROM tbl_analytics", conn);
            object result = exec.ExecuteScalar();

            int rowCount = 0;
            if (result != null)
            {
                rowCount = Convert.ToInt32(result);
                Console.WriteLine($"Number of rows: {rowCount}");
            }

            totalPage = (int)Math.Ceiling((double)rowCount / pageSize);
        }
        return new CrimePageAPI(l, totalPage);
    }
}
