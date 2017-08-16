using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/CategoryForm")]
    public ActionResult CategoryForm()
    {
      return View();
    }

    [HttpGet("/Categories")]
    public ActionResult CategoriesGet()
    {
      return View("Categories",Category.GetAll());
    }

    [HttpPost("/Categories")]
    public ActionResult Categories()
    {
      string categoryName = Request.Form["category"];
      Category newCategory = new Category(categoryName);
      newCategory.Save();

      return View(Category.GetAll());
    }

    [HttpGet("/CategoryDetails/{id}/TaskForm")]
    public ActionResult TaskForm(int id)
    {
      Category selectedCategory = Category.Find(id);
      return View(selectedCategory);
    }

    [HttpPost("/CategoryDetails/{id}")]
    public ActionResult CategoryDetailsPost(int id)
    {
      string taskDescription = Request.Form["task"];
      DateTime taskDateTime = DateTime.Parse(Request.Form["task-date"]);
      Task addedTask = new Task(taskDescription, id, taskDateTime);
      addedTask.Save();

      Dictionary<string, object> model = new Dictionary<string, object> ();

      Category selectedCategory = Category.Find(id);
      List<Task> categoryTaskList = selectedCategory.GetTasks();

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTaskList);

      return View("CategoryDetails", model);
    }

    [HttpGet("/CategoryDetails/{id}")]
    public ActionResult CategoryDetails(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();
      Category selectedCategory = Category.Find(id);
      List<Task> categoryTaskList = selectedCategory.GetTasks();

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTaskList);

      return View(model);
    }

    [HttpGet("/CategoryDetails/{id}/DueEarliest")]
    public ActionResult SortByDueEarliest(int id)
    {
      Category.SetSortType("date_ascending");

      Dictionary<string, object> model = new Dictionary<string, object> ();
      Category selectedCategory = Category.Find(id);
      List<Task> categoryTaskList = selectedCategory.GetTasks();

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTaskList);

      return View("CategoryDetails", model);
    }

    [HttpGet("/CategoryDetails/{id}/DueLatest")]
    public ActionResult SortByDueLatest(int id)
    {
      Category.SetSortType("date_descending");

      Dictionary<string, object> model = new Dictionary<string, object> ();
      Category selectedCategory = Category.Find(id);
      List<Task> categoryTaskList = selectedCategory.GetTasks();

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTaskList);

      return View("CategoryDetails", model);
    }

    [HttpGet("/CategoryDetails/{id}/Alphabetical")]
    public ActionResult SortAlphabetically(int id)
    {
      Category.SetSortType("alphabetical_order");

      Dictionary<string, object> model = new Dictionary<string, object> ();
      Category selectedCategory = Category.Find(id);
      List<Task> categoryTaskList = selectedCategory.GetTasks();

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTaskList);

      return View("CategoryDetails", model);
    }

    [HttpGet("/CategoryDetails/{id}/BackwardAlphabeticalOrder")]
    public ActionResult SortAlphabeticallyBackwards(int id)
    {
      Category.SetSortType("backwards_alphabet");

      Dictionary<string, object> model = new Dictionary<string, object> ();
      Category selectedCategory = Category.Find(id);
      List<Task> categoryTaskList = selectedCategory.GetTasks();

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTaskList);

      return View("CategoryDetails", model);
    }

    [HttpGet("/CategoryDetails/{id}/ClearAll")]
    public ActionResult ClearTasks(int id)
    {
      Task.DeleteAll(id);

      Dictionary<string, object> model = new Dictionary<string, object> ();

      Category selectedCategory = Category.Find(id);
      List<Task> categoryTaskList = selectedCategory.GetTasks();

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTaskList);

      return View("CategoryDetails", model);
    }

    [HttpGet("/CategoryDetails/{catId}/ClearTask/{taskId}")]
    public ActionResult DeleteTask(int catId, int taskId)
    {

      Task.DeleteTask(taskId);

      Dictionary<string, object> model = new Dictionary<string, object> ();

      Category selectedCategory = Category.Find(catId);
      List<Task> categoryTaskList = selectedCategory.GetTasks();

      model.Add("category", selectedCategory);
      model.Add("tasks", categoryTaskList);

      return View("CategoryDetails", model);
    }

    [HttpGet("/CategoryDetails/{id}/ClearCategory")]
    public ActionResult DeleteCategory(int id)
    {
      Category.DeleteCategory(id);
      List<Category> allCategories = Category.GetAll();
      Task.DeleteAll(id);
      return View("Categories", allCategories);
    }

  }
}
