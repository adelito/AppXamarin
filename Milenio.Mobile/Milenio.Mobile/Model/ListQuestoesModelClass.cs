using Mobile.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    public class ListQuestoesModelClass
    {
        public List<QuestionarioModelClass> questoes = new List<QuestionarioModelClass>();
        public  ListQuestoesModelClass(object Mylist)
        {
            if(Mylist is IList)
            {
              
            }

        }
    }
}
