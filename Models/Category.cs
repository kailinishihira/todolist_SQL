using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
  public class Category
  {
    private string _name;
    private int _id;
    private static string _sortType = "date_ascending";

    public Category(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }
    public override bool Equals (System.Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        return this.GetId().Equals(newCategory.GetId());
      }
    }
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }
    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public static void SetSortType(string sortType)
    {
      _sortType = sortType;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO `categories`  (`name`) VALUES (@name);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
    }

    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int CategoryId = rdr.GetInt32(0);
        string CategoryName = rdr.GetString(1);
        Category newCategory = new Category(CategoryName, CategoryId);
        allCategories.Add(newCategory);
      }
      return allCategories;
    }

    public static Category Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CategoryId = 0;
      string CategoryName ="";

      while (rdr.Read())
      {
        CategoryId = rdr.GetInt32(0);
        CategoryName = rdr.GetString(1);
      }
      Category newCategory = new Category(CategoryName,CategoryId);
      return newCategory;
    }

    public static void DeleteCategory(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM categories WHERE id = @thisId;";

      MySqlParameter categoryId = new MySqlParameter();
      categoryId.ParameterName = "@thisId";
      categoryId.Value = id;
      cmd.Parameters.Add(categoryId);

      cmd.ExecuteNonQuery();
    }

    public List<Task> GetTasks()
    {
      List<Task> allCategoryTasks = new List<Task>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      if (_sortType=="date_ascending")
      {
          cmd.CommandText =@"SELECT * FROM tasks WHERE category_id = @category_id ORDER BY due_date ASC;";
      }
      else if (_sortType == "date_descending")
      {
          cmd.CommandText = @"SELECT * FROM tasks WHERE category_id = @category_id ORDER BY due_date DESC;";
      }
      else if (_sortType == "alphabetical_order")
      {
        cmd.CommandText = @"SELECT * FROM tasks WHERE category_id = @category_id ORDER BY description ASC;";
      }
      else
      {
        cmd.CommandText = @"SELECT * FROM tasks WHERE category_id = @category_id ORDER BY description DESC;";
      }


      MySqlParameter categoryId = new MySqlParameter();
      categoryId.ParameterName = "@category_Id";
      categoryId.Value = this._id;
      cmd.Parameters.Add(categoryId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        int taskCategoryId = rdr.GetInt32(2);
        DateTime taskDateTime = rdr.GetDateTime(3);
        Task newTask = new Task(taskDescription,taskCategoryId, taskDateTime, taskId);
        allCategoryTasks.Add(newTask);
      }
      return allCategoryTasks;


    }
  }
}
