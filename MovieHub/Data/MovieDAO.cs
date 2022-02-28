using MovieHub.Models;
using Npgsql;
using NpgsqlTypes;


namespace MovieHub.Data;

public class MovieDao
{

    // performs all operations on the database. Get all, create, delete, get ons, search, etc
    private const string ConnectionString = @"host=db-postgresql-ams3-54359-do-user-9452846-0.b.db.ondigitalocean.com;port=25060;username=doadmin;password=l22MGuwQCl314eL7;database=rob";

    public static Movie MovieNow(int hallId)
    {
        //access the database
        using var connection = new NpgsqlConnection(ConnectionString);
        
        // SQL Query
        const string sql = "SELECT * FROM rob.public.\"Movie\" JOIN rob.public.\"MovieRuntime\" ON \"MovieRuntime\".\"MovieId\" = \"Movie\".\"Id\" WHERE \"MovieRuntime\".\"HallId\" = @hallId AND \"MovieRuntime\".\"Time\"::Time < now()::Time ORDER BY \"MovieRuntime\".\"Time\" DESC LIMIT 1";

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add("@hallId", NpgsqlDbType.Integer).NpgsqlValue = hallId;

        connection.Open();
        var reader = command.ExecuteReader();

        var movie = new Movie();

        if (!reader.HasRows) return movie;
        
        while (reader.Read())
        {
            // create a new movie object. Add it to the list to return
            movie.Title = reader.GetString(1);
        }
        return movie;
    }
    
    
    public static Movie MovieNext(int hallId)
    {

        //access the database
        using var connection = new NpgsqlConnection(ConnectionString);
        
        // SQL Query
        const string sql = "SELECT * FROM rob.public.\"Movie\" JOIN rob.public.\"MovieRuntime\" ON \"MovieRuntime\".\"MovieId\" = \"Movie\".\"Id\" WHERE \"MovieRuntime\".\"HallId\" = @hallId AND \"MovieRuntime\".\"Time\"::Time > now()::Time ORDER BY \"MovieRuntime\".\"Time\" LIMIT 1";

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add("@hallId", NpgsqlDbType.Integer).NpgsqlValue = hallId;

        connection.Open();
        var reader = command.ExecuteReader();

        var movie = new Movie();

        if (!reader.HasRows) return movie;
        
        while (reader.Read())
        {
            // create a new movie object. Add it to the list to return
            movie.Title = reader.GetString(1);
        }
        return movie;
    }
    
    // public static TimeSpan MovieNextTime(int hallId)
    // {
    //     //access the database
    //     using var connection = new NpgsqlConnection(ConnectionString);
    //     
    //     // SQL Query
    //     const string sql = "SELECT * FROM rob.public.\"Movie\" JOIN rob.public.\"MovieRuntime\" ON \"MovieRuntime\".\"MovieId\" = \"Movie\".\"Id\" WHERE \"MovieRuntime\".\"HallId\" = @hallId AND \"MovieRuntime\".\"Time\"::Time > now()::Time ORDER BY \"MovieRuntime\".\"Time\" LIMIT 1";
    //
    //     var command = new NpgsqlCommand(sql, connection);
    //     command.Parameters.Add("@hallId", NpgsqlDbType.Integer).NpgsqlValue = hallId;
    //
    //     connection.Open();
    //     var reader = command.ExecuteReader();
    //     
    //     var movieRuntime = new MovieRuntime();
    //
    //     if (!reader.HasRows) return movieRuntime.Time;
    //     
    //     while (reader.Read())
    //     {
    //         // create a new movie object. Add it to the list to return
    //         movieRuntime.Time = reader.GetTimeSpan(1);
    //     }
    //     return movieRuntime.Time;
    // }
}
