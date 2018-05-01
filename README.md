# ADO.NET_ReaderData_PerformanceTest
ADO.NET的效能測試，僅測試SQL讀取完畢，轉回程式端的效能。其中包含了DataTable.Load、IDataReader、DataAdapter.Fill的比較

測試項目
1.DataTable.Load(IDataReader)
2.DataTable.Load(IDataReader) - IDataReader使用CommandBehavior.SequentialAccess
3.IDataReader
4.IDataReader - 使用CommandBehavior.SequentialAccess
5.DataAdapter.Fill

測試內容
測試50次取平均值，回傳二個DataTable，第一個Table約90筆資料，第二個Table只有1筆資料

效能比較
1.DataTable.Load(IDataReader) - 4.386241878 ms
2.DataTable.Load(IDataReader) - IDataReader使用CommandBehavior.SequentialAccess - 0.118485948 ms
3.IDataReader - 0.117798434 ms
4.IDataReader - 使用CommandBehavior.SequentialAccess - 0.117798434 ms
5.DataAdapter.Fill - 2.84500376 ms

效能排名
1.IDataReader - 使用CommandBehavior.SequentialAccess
2.IDataReader
3.DataTable.Load(IDataReader) - IDataReader使用CommandBehavior.SequentialAccess
4.DataAdapter.Fill
5.DataTable.Load(IDataReader)
