import { ENDPOINT_API } from "../http-services/axios/config";
import { httpClient } from "../http-services/axios/httpClient";
import { DashboardData } from "../interfaces/dashboard.interface";

export const getDashboardData = async (): Promise<DashboardData> => {
  try {
    const response = await httpClient.get<DashboardData>(
      ENDPOINT_API.admin.dashboard
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching dashboard data", error);
    throw error;
  }
};