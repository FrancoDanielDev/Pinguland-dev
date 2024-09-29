using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MyExtesions
{
    public static string ToTime(this float f)
    {
        int minutes = Mathf.FloorToInt(f / 60);
        int seconds = Mathf.FloorToInt(f % 60);

        return minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
    }

    public static string ToTime(this int f)
    {
        int minutes = Mathf.FloorToInt(f / 60);
        int seconds = Mathf.FloorToInt(f % 60);

        return minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
    }

    public static string ToBoard(this List<string> list)
    {
        //Se crea una lista de strings, con la misma cantidad de elementos que la pasada por parámetro.
        List<string> _numbers = new List<string>();
        for (int i = 0; i < list.Count; i++)
        {
            if(i < 10)
                _numbers.Add($"#0{i + 1} | ");
            else
                _numbers.Add($"#{i + 1} | ");
        }

        //Hago un Zip para asignarle un número a cada jugador.
        List<string> result = _numbers.Zip(list, (n, s) => n + " " + s).ToList();
        
        //Convierto toda la lista en un string y lo devuelvo.
        string board = string.Join("\n", result.Select(x => x)).ToString();
        return board;
    }
}