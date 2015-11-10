<%@ webhandler Language="C#" class="FileManager" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET Program is a demonstration program , Not recommended for use directly in the actual project .
 *  If you decide to use the program directly , Please confirm the relevant security settings before using .
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using LitJson;
using System.Collections.Generic;

public class FileManager : IHttpHandler
{
	public void ProcessRequest(HttpContext context)
	{
		String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

		// Root path , Relative path 
        String rootPath = "/DataUser/RichTextFile/";
        
		// Root directory URL, You can specify an absolute path , Such as  http://www.yoursite.com/attached/
        String rootUrl = aspxUrl + "/DataUser/RichTextFile/";
        
		// Pictures extension 
		String fileTypes = "gif,jpg,jpeg,png,bmp";

		String currentPath = "";
		String currentUrl = "";
		String currentDirPath = "";
		String moveupDirPath = "";

		String dirPath = context.Server.MapPath(rootPath);
		String dirName = context.Request.QueryString["dir"];
		if (!String.IsNullOrEmpty(dirName)) {
			if (Array.IndexOf("image,flash,media,file".Split(','), dirName) == -1) {
				context.Response.Write("Invalid Directory name.");
				context.Response.End();
			}
			dirPath += dirName + "/";
			rootUrl += dirName + "/";
			if (!Directory.Exists(dirPath)) {
				Directory.CreateDirectory(dirPath);
			}
		}

		// According to path Parameters , Set each path and URL
		String path = context.Request.QueryString["path"];
		path = String.IsNullOrEmpty(path) ? "" : path;
		if (path == "")
		{
			currentPath = dirPath;
			currentUrl = rootUrl;
			currentDirPath = "";
			moveupDirPath = "";
		}
		else
		{
			currentPath = dirPath + path;
			currentUrl = rootUrl + path;
			currentDirPath = path;
			moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
		}

		// Sort form ,name or size or type
		String order = context.Request.QueryString["order"];
		order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

		// Not allowed .. Move to the parent directory 
		if (Regex.IsMatch(path, @"\.\."))
		{
			context.Response.Write("Access is not allowed.");
			context.Response.End();
		}
		// The last character is not /
		if (path != "" && !path.EndsWith("/"))
		{
			context.Response.Write("Parameter is not valid.");
			context.Response.End();
		}
		// Directory does not exist or is not a directory 
		if (!Directory.Exists(currentPath))
		{
			context.Response.Write("Directory does not exist.");
			context.Response.End();
		}

		// Directory traversal get file information 
		string[] dirList = Directory.GetDirectories(currentPath);
		string[] fileList = Directory.GetFiles(currentPath);

		switch (order)
		{
			case "size":
				Array.Sort(dirList, new NameSorter());
				Array.Sort(fileList, new SizeSorter());
				break;
			case "type":
				Array.Sort(dirList, new NameSorter());
				Array.Sort(fileList, new TypeSorter());
				break;
			case "name":
			default:
				Array.Sort(dirList, new NameSorter());
				Array.Sort(fileList, new NameSorter());
				break;
		}

		Hashtable result = new Hashtable();
		result["moveup_dir_path"] = moveupDirPath;
		result["current_dir_path"] = currentDirPath;
		result["current_url"] = currentUrl;
		result["total_count"] = dirList.Length + fileList.Length;
		List<Hashtable> dirFileList = new List<Hashtable>();
		result["file_list"] = dirFileList;
		for (int i = 0; i < dirList.Length; i++)
		{
			DirectoryInfo dir = new DirectoryInfo(dirList[i]);
			Hashtable hash = new Hashtable();
			hash["is_dir"] = true;
			hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
			hash["filesize"] = 0;
			hash["is_photo"] = false;
			hash["filetype"] = "";
			hash["filename"] = dir.Name;
			hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
			dirFileList.Add(hash);
		}
		for (int i = 0; i < fileList.Length; i++)
		{
			FileInfo file = new FileInfo(fileList[i]);
			Hashtable hash = new Hashtable();
			hash["is_dir"] = false;
			hash["has_file"] = false;
			hash["filesize"] = file.Length;
			hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
			hash["filetype"] = file.Extension.Substring(1);
			hash["filename"] = file.Name;
			hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
			dirFileList.Add(hash);
		}
		context.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(result));
		context.Response.End();
	}

	public class NameSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			FileInfo xInfo = new FileInfo(x.ToString());
			FileInfo yInfo = new FileInfo(y.ToString());

			return xInfo.FullName.CompareTo(yInfo.FullName);
		}
	}

	public class SizeSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			FileInfo xInfo = new FileInfo(x.ToString());
			FileInfo yInfo = new FileInfo(y.ToString());

			return xInfo.Length.CompareTo(yInfo.Length);
		}
	}

	public class TypeSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			FileInfo xInfo = new FileInfo(x.ToString());
			FileInfo yInfo = new FileInfo(y.ToString());

			return xInfo.Extension.CompareTo(yInfo.Extension);
		}
	}

	public bool IsReusable
	{
		get
		{
			return true;
		}
	}
}
