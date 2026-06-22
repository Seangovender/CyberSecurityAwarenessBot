/* DatabaseHelper.cs – MySQL Database Integration for Task Assistant
 * Module: PROG6221
 * Student Name: Sean Govender
 * Student Number: ST10491364
 * Date: 22 June 2026
 * Description: Handles all database operations for the Task Assistant.
 *              Provides CRUD (Create, Read, Update, Delete) functionality
 *              for cybersecurity tasks stored in a MySQL database.
 */

using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityChatbot_Part2
{
    
    // Handles all database operations for the Task Assistant feature.
    // Provides methods to add, retrieve, update, and delete tasks
    // from the MySQL database using parameterised queries for security.
   
    public class DatabaseHelper
    {
        // Private Fields

        
        // MySQL connection string containing server, database, username, and password.
        // Update the password to match your local MySQL installation.
        
        private string connectionString = "Server=localhost;Database=cybersecurity_bot;Uid=root;Pwd=YourNewPassword123;";

        // Public Methods 

        
        // It Adds a new task to the database.
        
        public bool AddTask(string title, string description, DateTime? reminderDate = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO tasks (title, description, reminder_date) 
                                     VALUES (@title, @description, @reminderDate)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Use parameterised queries to prevent SQL injection
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@reminderDate", reminderDate ?? (object)DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch
            {
                // Return false if the operation fails (e.g., connection issues)
                return false;
            }
        }

        
        // Retrieves all tasks from the database.
        
        public List<TaskItem> GetTasks(bool includeCompleted = false)
        {
            var tasks = new List<TaskItem>();
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = includeCompleted ?
                        "SELECT * FROM tasks ORDER BY created_at DESC" :
                        "SELECT * FROM tasks WHERE is_completed = FALSE ORDER BY created_at DESC";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TaskItem
                            {
                                Id = reader.GetInt32("id"),
                                Title = reader.GetString("title"),
                                Description = reader.GetString("description"),
                                ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? null : reader.GetDateTime("reminder_date"),
                                IsCompleted = reader.GetBoolean("is_completed")
                            });
                        }
                    }
                }
            }
            catch
            {
                // Return empty list if the operation fails
            }
            return tasks;
        }

        
        // Marks a task as completed in the database.
     
        public bool MarkTaskComplete(int taskId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Deletes a task from the database.
        
        public bool DeleteTask(int taskId)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM tasks WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }

    
    // Used to transfer task data between the database and the application.
    
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}

//Reference list:
//Oracle Corporation. 2026. MySQL Connector/NET Developer Guide.
//Available at: https://dev.mysql.com/doc/connector-net/en/ (Accessed: 22 June 2026)

// Stack Overflow. (2014).Parameterized Queries in C# with MySQL.
// Available at: https://stackoverflow.com/questions/750580/ (Accessed: 22 June 2026)