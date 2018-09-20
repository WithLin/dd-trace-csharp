#include <spdlog/sinks/rotating_file_sink.h>
#include <spdlog/sinks/stdout_sinks.h>
#include <filesystem>

#include "logging.h"
#include "util.h"

namespace trace {
std::shared_ptr<spdlog::logger> GetLogger() {
  static std::mutex mtx;

  auto logger = spdlog::get("dotnet-profiler");
  if (logger == nullptr) {
    std::unique_lock<std::mutex> lck(mtx);

    const auto programdata = GetEnvironmentValue(L"PROGRAMDATA");
    const auto dddir = std::filesystem::path(programdata)
                           .append("Datadog")
                           .append("logs")
                           .append("dotnet-profiler.log");

    std::string logfile;

    if (std::filesystem::exists(dddir)) {
      logfile = dddir.string();
    } else {
      logfile = R"(C:\ProgramData\Datadog\logs\dotnet-profiler.log)";
    }

    logger = spdlog::rotating_logger_mt("dotnet-profiler", logfile,
                                        1024 * 1024 * 5, 3);
  }
  return logger;
}
}  // namespace trace
