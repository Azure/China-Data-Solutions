write("Start Executing R Script", file="./log.csv", append = TRUE)

write("Installing Packages", file="./log.csv", append = TRUE)
list.of.packages <- c("igraph", "readr", "ggplot2", "jsonlite")
new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(new.packages)) install.packages(new.packages)

write("Packages Installed", file="./log.csv", append = TRUE)

library(mrsdeploy)
library(igraph)
library(readr)
library(ggplot2)
library(jsonlite)

write("Libary Attached", file="./log.csv", append = TRUE)

Find_KOL <-  function(fileStr)
{
  library(igraph)
  library(readr)
  library(ggplot2)
  library(jsonlite)
  data <- as.data.frame(fromJSON(fileStr))
  
  # data file I used had four columns named ego, alter, social_tie, task_tie. I reshaped and renamed it to fit your data

  colnames(data) <- c("ego", "alter","weight")
  
  # convert into a graph object
  rsn <- graph.data.frame(rdata) 
  
  # computing the out degree centrality. igraph computes the number of ties but not centrality, so I normalized it with H = (n-1) as stated in networkx document

  pr <- page_rank(rsn)
  prvals <- sort(pr[[1]],decreasing = T)  %>% as.data.frame()
  out_pr <- cbind(rownames(prvals), prvals) %>% as.data.frame()
  colnames(out_pr) <-  c("uid", "value")
  rownames(out_pr) <- NULL
  answer <- toJSON(out_d)
  return(answer)
}
write.csv("Start Remote Login", "./log.csv", append=T)

tryCatch(remoteLogin("http://localhost:12800", 
                     username = "admin",
                     password = "Passw0rd123!",
                     session = FALSE), error=function(e){
                       write("Error Login", file="./log.csv", append = TRUE)
                       write(e, file="./log.csv", append = TRUE)
                     })

tryCatch(getService("KOLService"), error=function(e){
  publishService("KOLService", 
                 code=Find_KOL, 
                 inputs=list(fileStr="character"),
                 outputs = list(answer="character"),
                 v="v1.0.0")
  write("Publish Service Done", file="./log.csv", append = TRUE)				
})