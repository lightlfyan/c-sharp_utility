using System;
using System.Collections;
using System.Linq;
using System.Linq.Parallel;
using System.Net;
using System.Threading.Tasks;

public class Program {
    private static Task<byte[]> startDownload(string url){
        var tcs = new TaskCompletionSource<byte[]>(url);
        var wc = new WebClient();
        wc.DownloadDataCompleted += (sender, e) =>
        {
            if(e.UserState == tcs){
                if(e.Cancelled) tcs.TrySetCanceled();
                else if(e.Error!=null) tcs.TrySetException(e.Error);
                else tcs.TrySetResult(e.Result);
            }
        };
        wc.DownloadDataAsync(new Uri(url), tcs);
        return tcs.Task;
    }

    private static void FinishDownload(string url, byte[] bytes){
        Console.WriteLine("read {0} from {1}", bytes.Length, url);
    }

    static void test(){
        string[] urls = new string[]{"http://www.baidu.com"};
        var results = from url in urls.AsParallel()
            select new WebClient().DownloadData(url);

        results.ForAll(result => Console.WriteLine(result.Length));

        urls.RunAsync(
            url => startDownload(url),
            task => FinishDownload(task.AsyncState.ToString(), task,Result)
            );
    }

    public static void Main(){
        test();
    }
}
