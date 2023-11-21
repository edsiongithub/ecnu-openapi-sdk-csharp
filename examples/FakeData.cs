 using SqlSugar;
 public class FakeData
 {
     [SugarColumn(IsPrimaryKey = true)]
     public int Id { get; set; }
     public string colString1 { get; set; }
     public string colString2 { get; set; }
     public string colString3 { get; set; }
     public string colString4 { get; set; }
     public long colInt1 { get; set; }
     public long colInt2 { get; set; }
     public long colInt3 { get; set; }
     public long colInt4 { get; set; }
     public float colFloat1 { get; set; }
     public float colFloat2 { get; set; }
     public float colFloat3 { get; set; }
     public float colFloat4 { get; set; }
     public DateTime colSqlTime1 { get; set; }
     public DateTime colSqlTime2 { get; set; }
     public DateTime colSqlTime3 { get; set; }
     public DateTime colSqlTime4 { get; set; }
 }