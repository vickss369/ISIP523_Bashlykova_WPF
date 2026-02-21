using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12
{
    public partial class basepart_
    {
        public List<string> Specs
        {
            get
            {
                var specs = new List<string>();

                if (cpu_ != null)
                {
                    specs.Add($"Сокет: {cpu_.socket_?.name}");
                    specs.Add($"Ядер: {cpu_.numberofcores}");
                    specs.Add($"Частота: {cpu_.basecorefrequency}–{cpu_.maxcorefrequency} ГГц");
                    specs.Add($"Кэш L3: {cpu_.cachel3} МБ");
                    specs.Add($"TDP: {cpu_.thermalpower} Вт");
                }

                if (motherboard_ != null)
                {
                    specs.Add($"Сокет: {motherboard_.socket_?.name}");
                    specs.Add($"Форм-фактор: {motherboard_.formfactor_?.name}");
                    specs.Add($"Тип памяти: {motherboard_.memorytype_?.name}");
                    specs.Add($"Слоты RAM: {motherboard_.memoryslots}");
                    specs.Add($"PCI слоты: {motherboard_.pcislots}");
                }

                if (gpu_ != null)
                {
                    specs.Add($"Видеопамять: {gpu_.videomemory} ГБ");
                    specs.Add($"Частота чипа: {gpu_.chipfrequency} МГц");
                    specs.Add($"Шина памяти: {gpu_.memorybus} бит");

                    if (gpu_.recommendpower != null)
                        specs.Add($"Реком. БП: {gpu_.recommendpower} Вт");
                }

                if (ram_ != null)
                {
                    specs.Add($"Тип: {ram_.memorytype_?.name}");
                    specs.Add($"Объём: {ram_.capacity} ГБ");
                    specs.Add($"Частота: {ram_.ghz} МГц");
                    specs.Add($"Модулей: {ram_.count}");
                    specs.Add($"Тайминги: {ram_.timings}");
                }

                if (powersupply_ != null)
                {
                    specs.Add($"Мощность: {powersupply_.power} Вт");
                }

                if (storagedevice_ != null)
                {
                    specs.Add($"Объём: {storagedevice_.capacity} ГБ");

                    if (storagedevice_.storagedevicetype_ != null)
                        specs.Add($"Тип: {storagedevice_.storagedevicetype_.name}");

                    if (storagedevice_.storagedeviceinterface_ != null)
                        specs.Add($"Интерфейс: {storagedevice_.storagedeviceinterface_.name}");

                    if (storagedevice_.hdd_ != null)
                    {
                        specs.Add("Тип: HDD");
                    }

                    if (storagedevice_.ssd_ != null)
                    {
                        specs.Add("Тип: SSD");
                    }
                }

                if (processorcooler_ != null)
                {
                    specs.Add($"Тепловые трубки: {processorcooler_.heatpipes}");
                    specs.Add($"Обороты: {processorcooler_.minspeed}-{processorcooler_.maxspeed} RPM");
                    specs.Add($"Шум: {processorcooler_.noiselevel} дБ");

                    var sockets = processorcooler_.socketprocessorcooler_
                        ?.Select(s => s.socket_?.name);

                    if (sockets != null && sockets.Any())
                        specs.Add($"Сокеты: {string.Join(", ", sockets)}");
                }

                if (case_ != null)
                {
                    specs.Add($"Размер: {case_.casesize_?.name}");
                    specs.Add($"Вентиляторы: {case_.fans}");

                    var factors = case_.boardformfactorcase_?.Select(f => f.formfactor_?.name);

                    if (factors != null && factors.Any())
                        specs.Add($"Форм-факторы: {string.Join(", ", factors)}");
                }

                return specs;
            }
        }
    }
}
