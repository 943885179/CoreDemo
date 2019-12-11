# .net core 3.0内置System.Text.Json;
## 转字符串：JsonSerializer.Serialize(object);
## 转实体：JsonSerializer.Deserialize<T>(jsonStr);
## 枚举类型字段转字符串时候显示枚举名称而非值：添加[JsonConverter(typeof(JsonStringEnumConverter))] 然后转化后就是名称，而且字符串是名称也也可以转实体
## 转化中文等会转为ASCII,保留中文需要添加` new JsonSerializerOptions() {Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)};`