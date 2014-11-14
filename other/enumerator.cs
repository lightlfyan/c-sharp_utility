using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Direction
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

public class WeatherData
{
    public double Temperature {get; set; }
    public int WindSpeed {get; set;}
    public Direction WindDirection {get; set;}
    public override string ToString(){
        return string.Format("{0}, {1}, {1}", Temperature, WindSpeed, WindDirection);
    }
}

public class WeatherDataStream: IEnumerable<WeatherData>
{
    private Random generator = new Random();
    public WeatherDataStream(string localtion){

    }

    private IEnumerator<WeatherData> getElements(){
        for(int i=0; i<100; i++){
            yield return new WeatherData
            {
                Temperature = generator.NextDouble() * 90,
                WindSpeed = generator.Next(70),
                WindDirection = (Direction)generator.Next(7)
            };
        }
    }

    #region IEnumerable<WeatherData> Members
    public IEnumerator<WeatherData> GetEnumerator(){
        return getElements();
    }
    #endregion

    #region IEnumerable Members
    System.Collections.IEnumerator
    System.Collections.IEnumerable.GetEnumerator(){
        return getElements();
    }
    #endregion
}

public class Ltest: IEnumerable<int> {
    private int index = 0;

    public int[] l = new int[]{1,2,3,4,5};

    public IEnumerator<int> getElements(){
        while(index < l.Count()){
            yield return l[index];
            index++;
        }
    }

    #region IEnumerable<int> Members
    public IEnumerator<int> GetEnumerator(){
        return getElements();
    }
    #endregion

    #region IEnumerable Members
    System.Collections.IEnumerator
    System.Collections.IEnumerable.GetEnumerator(){
        return getElements();
    }
    #endregion

}

public struct Info: IComparable<Info>, IComparable{
    private string data;

    #region IComparable<Info> Members
    public int CompareTo(Info other){
        return data.CompareTo(other.data);
    }
    #endregion

    #region IComparable Members
    int IComparable.CompareTo(object obj){
        if(obj is Info){
            return CompareTo(obj as Info);
        } else 
        throw new ArgumentException("compare object not info"):
    }
    #endregion
}

public class Gen {



    public static void Main(){
        var warmDays = from item in 
        new WeatherDataStream("china")
        where item.Temperature > 80
        select item;

        Ltest l = new Ltest();

        l.ToList().ForEach((n) => Console.WriteLine(n));

        //warmDays.ToList().ForEach((n)=> Console.WriteLine(n));

        Console.WriteLine("ok");
    }
}