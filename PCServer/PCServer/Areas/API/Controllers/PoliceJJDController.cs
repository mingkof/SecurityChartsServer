using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System.Text;

namespace SHSecurityServer.Controllers
{
    //参考 https://stackoverflow.com/questions/37280303/swagger-500-error-in-web-api

    [Produces("application/json")]
    [Route("api/jjd")]
    public class PoliceJJDController : Controller
    {
        private readonly IJJDRepository _jjdRepository;
        private readonly ILogger _logger;
        public PoliceJJDController (IJJDRepository jjdRepos, ILogger<PoliceJJDController> logger)
        {
            _jjdRepository = jjdRepos;
            _logger = logger;
        }

        // GET: api/PoliceJJD
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Get APi PoliceJJD");

            return Ok(_jjdRepository.FindList(p => true, "", false));
        }

        // GET: api/PoliceJJD/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            JPoliceJJD _jjd = _jjdRepository.GetCm(id);

            if (_jjd == null)
                return NotFound();

            return Ok(_jjd);
        }
        
        // POST: api/PoliceJJD
        [HttpPost]
        public IActionResult Post([FromBody]db_jjd value)
        {
            if(value == null || value.jjdid == null || !ModelState.IsValid)
                return BadRequest();

            var query = _jjdRepository.Get(value.jjdid);

            if (query != null)
                return BadRequest();

            _jjdRepository.Add(value);

            return Ok();
        }
        
        // PUT: api/PoliceJJD/5
        //[HttpPut("{jjdId}")]
        //public IActionResult Put(string jjdId, [FromBody]db_jjd value)
        //{
        //    if (value == null  || !ModelState.IsValid)
        //        return BadRequest();

        //    var query = _jjdRepository.Get(jjdId);

        //    if (query == null)
        //        return NotFound();

        //    query.af_addr = value.af_addr;

        //    _jjdRepository.Update(query);

        //    return Ok();
        //}
        

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{jjdId}")]
        public IActionResult Delete(string jjdId)
        {
            if (jjdId == null)
                return BadRequest();

            var query = _jjdRepository.Get(jjdId);
            if (query == null)
                return NotFound();

            _jjdRepository.Remove(query);

            return Ok();
        }


        [HttpGet("/api/test/oracle")]
        public IActionResult TestOracle()
        {
            //var connectionString = "user id=ja110_share;password=ja110_share;data source=10.17.56.128:1521/ja110";
            //ServerDBExt.Database.IDatabase db = new DatabaseOracle(connectionString);

            //if (db.DoEnsureOpen((string res) => {

            //    _logger.LogInformation("Oracle Connect Fail: " + res);

            //}))
            //{

            //    var res = db.Query("SELECT * FROM RPT.PD_POLICE_ALL_DATA", null);

            //    foreach (var item in res)
            //    {
            //        var dic = item;

            //        //StringBuilder sb = new StringBuilder();

            //        //foreach (var dicitem in dic)
            //        //{
            //        //    sb.Append(dicitem.Key + ":" + dicitem.Value + "   ");
            //        //}

            //    }

            //    db.DoEnsureClose();
            //}

            return Ok();
        }
    }
}
