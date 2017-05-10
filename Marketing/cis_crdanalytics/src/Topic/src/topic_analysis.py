#encoding=utf-8

import jieba
import jieba.posseg as posseg

class TopicAnalysis:
	def __init__(self):

		jieba.load_userdict("./dicts/topic/user.dict")
		self.__root_filepath = "./dicts/topic/"
		self.topic_dict = {}
		self.__news_dict = self.__get_topic_dict(self.__root_filepath + "news_dict.txt")
		self.topic_dict["行业新闻"] = self.__news_dict
		self.__market_dict = self.__get_topic_dict(self.__root_filepath + "market_dict.txt")    
		self.topic_dict["市场推广"] = self.__market_dict   	
		self.__user_experience_dict = self.__get_topic_dict(self.__root_filepath + "user_experience_dict.txt")
		self.topic_dict["用户体验"] = self.__user_experience_dict
		self.__cost_dict = self.__get_topic_dict(self.__root_filepath + "cost_dict.txt")
		self.topic_dict["费用"] = self.__cost_dict
		self.__purchase_option_dict = self.__get_topic_dict(self.__root_filepath + "purchase_option_dict.txt")
		self.topic_dict["购买选择"] = self.__purchase_option_dict
		self.__parts_dict = self.__get_topic_dict(self.__root_filepath + "parts_dict.txt")
		self.topic_dict["配件"] = self.__parts_dict
		self.__system_dict = self.__get_topic_dict(self.__root_filepath + "system_dict.txt")
		self.topic_dict["系统"] = self.__system_dict
		self.__performance_dict = self.__get_topic_dict(self.__root_filepath + "performance_dict.txt")
		self.topic_dict["性能"] = self.__performance_dict
		self.__service_dict = self.__get_topic_dict(self.__root_filepath + "service_dict.txt")
		self.topic_dict["服务"] = self.__service_dict
		self.__appearance_dict = self.__get_topic_dict(self.__root_filepath + "appearnce_dict.txt")
		self.topic_dict["外观"] = self.__appearance_dict
		self.__evaluation_dict = self.__get_topic_dict(self.__root_filepath + "evaluation_dict.txt")
		self.topic_dict["评测"] = self.__evaluation_dict
		self.__product_sale_dict = self.__get_topic_dict(self.__root_filepath + "product_sale_dict.txt")
		self.topic_dict["产品售卖"] = self.__product_sale_dict
		#self.__purchase_evaluation_dict = self.get_topic_dict(self.__root_filepath + "purchase_evaluation_dict.txt")
		#self.topic_dict["购买评价"] = self.__purchase_evaluation_dict
		self.__purpose_dict = self.__get_topic_dict(self.__root_filepath + "purpose_dict.txt")
		self.topic_dict["用途"] = self.__purpose_dict
		self.__product_information_dict = self.__get_topic_dict(self.__root_filepath + "product_information_dict.txt")
		self.topic_dict["产品信息"] = self.__product_information_dict

	def hit_num_analysis_file(self, filepath_in, filepath_out = None, encoding = 'utf-8', print_show = False):
		#open(filepath_out, "w")
	
		with open(filepath_in, "r") as f:
			line_number = 0
			for line in f:
				line_number +=1 
				seg_result = posseg.lcut(line.strip())
				hit_num_dict = {}
				hit_num_dict = self.get_each_hit_num_in_dict(seg_result)
				if print_show:
					self.__write_runout_file(filepath_out, str(line_number) + " : " + line + "\n")
					self.__write_runout_file(filepath_out, str(hit_num_dict) + "\n\n\n")			

		f.close()
		#filepath_out.close()

	@staticmethod
	def __write_runout_file(path, info, encoding="utf-8"):
		with open(path, "a") as f:
			f.write("%s" % info)
					
	

	def __get_topic_dict(self, path, encoding="utf-8"):
		#topic_dict = {}
		wordSet = set()
		with open(path, "r") as f:
			for line in f:
				word = line.strip()
				if len(word) == 0:
					continue

				wordSet.add(word)
				#print wordSet
		f.close()
		#topic_dict[name] = wordSet
		#return topic_dict	
		return wordSet

	def is_word_in_topic_dict(self, word, set_dict) :
		if word in set_dict:
			return True
		return False



	def get_hit_num_in_set(self, seg_result, set_dict):
		hit_number = 0
		for i in range(len(seg_result)):
			if self.is_word_in_topic_dict(seg_result[i].word.encode('utf-8'), set_dict):
		
				hit_number += 1

		return hit_number

	def get_each_hit_num_in_dict(self, seg_result):
		hit_num_dict = {}
		for key, value in self.topic_dict.items():
			hit_number = self.get_hit_num_in_set(seg_result, self.topic_dict[key])
			hit_num_dict[key] = hit_number
		return hit_num_dict		
