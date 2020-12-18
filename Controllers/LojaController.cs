using System;
using System.Collections.Generic;
using System.Linq;
using EVendas.Mapper;
using EVendas.Service.Interfaces;
using EVendas.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EVendasLoja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LojaController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ILogger<LojaController> _logger;

        public LojaController(IProdutoService produtoService, ILogger<LojaController> logger)
        {
            _produtoService = produtoService;
            _logger = logger;
        }


        [HttpPost]       
        [Route("VenderProduto")]        
        public IActionResult VenderProduto([FromBody] ProdutoVendaResponse produtoVendaResponse)
        {
            try
            {
                var produto = _produtoService.VenderProduto(produtoVendaResponse.ToEntityProdutoVenda());
                return Ok(produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, ex.InnerException);
                return BadRequest(ex.Message);
            }
            
        }

        
        [HttpGet]
        [Route("ListarProdutos")]        
        public IActionResult ListarProdutos()
        {
            try
            {
                IEnumerable<ProdutoResponse> produtos = _produtoService.ListarProdutos().Select(x=>x.ToProdutoResponse());
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, ex.InnerException);
                return BadRequest(new { Error = "Api unavailable" });
            }

        }
    }
}
