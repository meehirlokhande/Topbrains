using System;
using System.Collections.Generic;

class SortedArrays{

    public static T[] Merge<T>(T[] a,T[] b) where T:IComparable<T>{
        T[] merged = new T[a.Length + b.Length];
        int i=0,j=0,k=0;

        while(i<a.Length && j < b.Length){
            if(a[i].CompareTo(b[j]) < 0){
                merged[k++] = a[i++];
            }else{
                merged[k++] = b[j++];
            }
        }

        while(i< a.Length){
            merged[k++] = a[i++];
        }

        while(j<b.Length){
            merged[k++] = b[j++];
        }
        return merged;

    }



    public static void Main(string[] args){
        int[] arr1 = new int[]{1,3,5,7,9};
        int[] arr2 = new int[]{2,4,6,8,10};

        int[] result = Merge(arr1,arr2);

        Console.WriteLine(string.Join(" ", result));


    }
}