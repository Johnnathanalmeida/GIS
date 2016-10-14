using GISModel.DTO.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class BaseController : Controller
    {

        public RetornoJSON TratarRetornoValidacaoToJSON()
        {

            string msgAlerta = string.Empty;
            foreach (ModelState item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    foreach (System.Web.Mvc.ModelError i in item.Errors)
                    {
                        msgAlerta += i.ErrorMessage;
                    }
                }
            }

            return new RetornoJSON()
            {
                Alerta = msgAlerta,
                Erro = string.Empty,
                Sucesso = string.Empty
            };

        }

	}
}