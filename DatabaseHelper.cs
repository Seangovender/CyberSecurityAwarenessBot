/* ========================================================================
 * Database Helper – MySQL Integration for Task Assistant
 * Module: PROG6221
 * Student Name: [YOUR FULL NAME]
 * Student Number: ST10491364
 * Date: June 2026
 * ========================================================================
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberSecurityChatbot_Part2
{
    public class DatabaseHelper
    {
        // ⚠️ UPDATE THIS WITH YOUR MYSQL PASSWORD!
        private string connectionString = "Server=localhost;Database=cybersecurity_bot;Uid=root;Pwd=YourNewPassword123;";

        // ADD a new task
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
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@reminderDate", reminderDate.HasValue ? reminderDate.Value : (object)DBNull.Value);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB Error: {ex.Message}");
                return false;
            }
        }

        // GET all tasks (active only by default)
        public List<TaskItem> GetTasks(bool includeCompleted = false)
        {
            List<TaskItem> tasks = new List<TaskItem>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = includeCompleted ?
                        "SELECT * FROM tasks ORDER BY created_at DESC" :
                        "SELECT * FROM tasks WHERE is_completed = FALSE ORDER BY created_at DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TaskItem
                            {
                                Id = reader.GetInt32("id"),
                                Title = reader.GetString("title"),
                                Description = reader.GetString("description"),
                                ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ?
                                    null : reader.GetDateTime("reminder_date"),
                                IsCompleted = reader.GetBoolean("is_completed")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB Error: {ex.Message}");
            }
            return tasks;
        }

        // MARK task as complete
        public bool MarkTaskComplete(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        // DELETE a task
        public bool DeleteTask(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM tasks WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }
    }

    // TaskItem class to represent a task
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}