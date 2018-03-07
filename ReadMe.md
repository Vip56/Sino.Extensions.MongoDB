## Sino.Extensions.DocumentDB
为了提供除MySQL外其他仓储的实现版本，这里采用了Azure的DocumentDB进行了封装，其中1.x.x为采用官方SDK封装的版本，因为其在Docker存在性能问题，所以这里推荐使用版本2.x.x由MongoDB实现的仓储接口。   

[![Build status](https://ci.appveyor.com/api/projects/status/3prggrna3nh5k0em?svg=true)](https://ci.appveyor.com/project/vip56/sino-extensions-mongodb)
[![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg?style=plastic)](https://www.nuget.org/packages/Sino.Extensions.MongoDB)   

## 使用方法
```
Install-Package Sino.Extensions.MongoDB
```

在`Startup`中进行配置，
```
services.AddDocumentDB(host, userName, password, dataBase, bool noTotal = false);
```

## 注意点
### 关于多条件查询
因为MongoDB的IQueryable在利用Where重复嵌套多个查询条件的情况下存在问题，所以这里采用了表达式转换的方式将多个条件拼接为一个表达式，当然由于其使用场景，所以当前
仅支持了部分方式，以下将进行说明：  

方法调用形式
```
querys.Add(f => f.District.Contains("2"));
```  

二元操作
```
querys.Add(f => f.Total > 2);
```

特别注意不支持以下这种And操作
```
querys.Add(f => f.District.Contains("2") && f.LastName.Contains("1"));
```  

### 关于GetListAsync的Total问题
因为MongoDb暂不支持以IQueryable的方式计算total，同时也是避免这部分的性能损耗，所以这里total的算法进行了特别处理，
当用户根据skip和count去获取数据的时候，内部进行Take的时候将会获取count + 1条数据，如果最终获取的数据大于count，则
返回的`total=skip + count * 2`，否则返回`total=skip + count`，这样前端可以根据其判断是否能够继续分页。  
如果必须计算total，建议利用`Collection.Count`方式进行过滤计算。

### 关于时间问题
这里建议不要采用DateTime类型的时间，因为MongoDB存储的时间为UTC+0，所以大家会发现最终时间会少8个小时，为了避免应对这个问题所消耗的时间和精力，建议搭建采用long
类型保存时间，在最终显示的时候进行格式化。


## 版本更新记录
* 2018.3.7 支持asp.net core 2.0 by y-z-f