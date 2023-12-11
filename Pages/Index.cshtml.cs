using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Web;

namespace GCCode.Net.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string FIleName { get; set; } = string.Empty;

        public void OnGet()
        {
            if (Request.Query.ContainsKey("file"))
            {
                StringValues somestr22;
                Request.Query.TryGetValue("file", out somestr22);
                
                if (string.IsNullOrEmpty(somestr22))
                {
                    Redirect("/Index");
                }
                else
                {
                    FIleName = WorkClasses.Helpers.DecodeNonAscIItring(somestr22);
                }


            }







        }

        public IActionResult OnPostLoad()
        {




            var input_file = Request.Form.Files["inputFile"];

            if (input_file != null)
            {

                var file_directory = Path.Combine(Environment.CurrentDirectory, "wwwroot", "input_files");

                DeleteFilesFromDirectory(file_directory);

                System.IO.Directory.CreateDirectory(file_directory);


                var file = Path.Combine(file_directory, input_file?.FileName ?? "");

                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    input_file?.CopyTo(fileStream);

                    fileStream.Close();
                }


            }
            else
            {
                Console.WriteLine("Не удолось загрузить файл");
            }

            

            return Redirect("/Index?file=" + WorkClasses.Helpers.EncodeNonAscIItring(input_file?.FileName ?? ""));

                
        }

        public PartialViewResult OnGetCanvas_Partial()
        {
            if (Request.Query.ContainsKey("file"))
            {
                StringValues somestr22;
                Request.Query.TryGetValue("file", out somestr22);

                if (string.IsNullOrEmpty(somestr22))
                {
                    Redirect("/Index");
                }
                else
                {
                    FIleName = somestr22;
                }

                return Partial("_CanvasPartial", GetCanvasModel(FIleName));
            }
            else
            {
              return  Partial("_CanvasPartial", default);

            }




            
        }

        private (List<List<WorkClasses.Parser.Point>> points, WorkClasses.Parser.Point max_point) GetCanvasModel(string filename)
        {
            var filepath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "input_files", filename);

            var code = WorkClasses.Parser.ParseGcode(filepath);
            //var info = WorkClasses.Parser.Get_Points(code);
            var dok = WorkClasses.Parser.GetPointsByLayers(code);




            return dok;
        }


        private static void DeleteFilesFromDirectory(string directory)
        {

            System.IO.DirectoryInfo di = new DirectoryInfo(directory);

            if (di.Exists)
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }


            }
            else
            {

                Console.Error.WriteLine($"Не удалось найти директорию {directory}");
            }



        }


    }
}
