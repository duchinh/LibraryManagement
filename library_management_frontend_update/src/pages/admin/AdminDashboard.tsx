import React, { useState, useEffect } from "react";
import { Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ChartOptions,
} from "chart.js";
import { DashboardData } from "../../interfaces/dashboard.interface";
import { getDashboardData } from "../../services/adminService";
import { ChevronDown, ChevronUp } from "lucide-react";
import ChartDataLabels from "chartjs-plugin-datalabels";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ChartDataLabels
);

const DashboardPage: React.FC = () => {
  const [dashboardData, setDashboardData] = useState<DashboardData | null>(
    null
  );
  const [showMostBorrowedChart, setShowMostBorrowedChart] = useState(false);
  const [showRequestChart, setShowRequestChart] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await getDashboardData();
        setDashboardData(data);
      } catch (error) {
        console.error("Error fetching dashboard data:", error);
      }
    };
    fetchData();
  }, []);

  const mostBorrowedBooks = dashboardData?.mostBorrowedBooks || [];
  const userActivities = dashboardData?.userActivities || [];

  const mostBorrowedBooksChartData = {
    labels: mostBorrowedBooks.map((book) => book.title),
    datasets: [
      {
        label: "Most Borrowed Books",
        data: mostBorrowedBooks.map((book) => book.borrowCount),
        backgroundColor: "rgba(75, 192, 192, 0.5)",
        borderColor: "rgba(75, 192, 192, 1)",
        borderWidth: 1,
        barPercentage: 0.5,
      },
    ],
  };

  const requestChartData = {
    labels: userActivities.map((u) => u.userName),
    datasets: [
      {
        label: "Requests Made",
        data: userActivities.map((u) => u.requestsMade),
        backgroundColor: "rgba(153, 102, 255, 0.5)",
        borderColor: "rgba(153, 102, 255, 1)",
        borderWidth: 1,
        barPercentage: 0.5,
      },
      {
        label: "Requests Done",
        data: userActivities.map((u) => u.requestsDone),
        backgroundColor: "rgba(75, 192, 192, 0.5)",
        borderColor: "rgba(75, 192, 192, 1)",
        borderWidth: 1,
        barPercentage: 0.5,
      },
      {
        label: "Requests Pending",
        data: userActivities.map((u) => u.requestsPending),
        backgroundColor: "rgba(255, 206, 86, 0.5)",
        borderColor: "rgba(255, 206, 86, 1)",
        borderWidth: 1,
        barPercentage: 0.5,
      },
      {
        label: "Requests Rejected",
        data: userActivities.map((u) => u.requestsRejected),
        backgroundColor: "rgba(255, 99, 132, 0.5)",
        borderColor: "rgba(255, 99, 132, 1)",
        borderWidth: 1,
        barPercentage: 0.5,
      },
    ],
  };

  const chartOptions: ChartOptions<"bar"> = {
    plugins: {
      datalabels: {
        display: true,
        color: "black",
        font: {
          weight: "bold",
          size: 14,
        },
        formatter: (value: number) => value.toString(),
        offset: 10,
      },
    },
    responsive: true,
    scales: {
      x: {
        ticks: {
          autoSkip: true,
          maxRotation: 0,
          minRotation: 0,
        },
      },
    },
  };

  return (
    <div className="p-6">
      {dashboardData ? (
        <>
          <h1 className="text-3xl font-bold mb-6">Admin Dashboard</h1>

          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
            <div className="stat-item bg-white p-4 shadow rounded">
              <h3 className="text-lg font-semibold">Total Books</h3>
              <p className="text-2xl">{dashboardData.totalBooks}</p>
            </div>
            <div className="stat-item bg-white p-4 shadow rounded">
              <h3 className="text-lg font-semibold">Total Borrowed Books</h3>
              <p className="text-2xl">{dashboardData.totalBorrowedBooks}</p>
            </div>
            <div className="stat-item bg-white p-4 shadow rounded">
              <h3 className="text-lg font-semibold">Total Users</h3>
              <p className="text-2xl">{dashboardData.totalUsers}</p>
            </div>
            <div className="stat-item bg-white p-4 shadow rounded">
              <h3 className="text-lg font-semibold">
                Total Borrowing Requests
              </h3>
              <p className="text-2xl">{dashboardData.totalBorrowingRequests}</p>
            </div>
          </div>

          <div className="chart-container mb-8 bg-white p-4 shadow rounded">
            <h2
              className="text-xl font-semibold mb-4 cursor-pointer select-none flex items-center"
              onClick={() => setShowMostBorrowedChart((prev) => !prev)}
            >
              Most Borrowed Books
              {showMostBorrowedChart ? (
                <ChevronUp className="ml-2" size={18} />
              ) : (
                <ChevronDown className="ml-2" size={18} />
              )}
            </h2>
            {showMostBorrowedChart && (
              <Bar data={mostBorrowedBooksChartData} options={chartOptions} />
            )}
          </div>

          <div className="chart-container mb-8 bg-white p-4 shadow rounded">
            <h2
              className="text-xl font-semibold mb-4 cursor-pointer select-none flex items-center"
              onClick={() => setShowRequestChart((prev) => !prev)}
            >
              Requests Made by Users
              {showRequestChart ? (
                <ChevronUp className="ml-2" size={18} />
              ) : (
                <ChevronDown className="ml-2" size={18} />
              )}
            </h2>
            {showRequestChart && (
              <Bar data={requestChartData} options={chartOptions} />
            )}
          </div>

          <div className="user-activities mb-8 bg-white p-4 shadow rounded">
            <h2 className="text-xl font-semibold mb-4">User Activities</h2>
            <table className="min-w-full table-auto border-collapse">
              <thead>
                <tr>
                  <th className="px-4 py-2 border-b text-left">UserName</th>
                  <th className="px-4 py-2 border-b text-left">
                    Requests Made
                  </th>
                  <th className="px-4 py-2 border-b text-left">
                    Requests Done
                  </th>
                  <th className="px-4 py-2 border-b text-left">
                    Requests Pending
                  </th>
                  <th className="px-4 py-2 border-b text-left">
                    Requests Rejected
                  </th>
                </tr>
              </thead>
              <tbody>
                {userActivities.map((activity, index) => (
                  <tr key={index}>
                    <td className="px-4 py-2 border-b">{activity.userName}</td>
                    <td className="px-4 py-2 border-b">
                      {activity.requestsMade}
                    </td>
                    <td className="px-4 py-2 border-b">
                      {activity.requestsDone}
                    </td>
                    <td className="px-4 py-2 border-b">
                      {activity.requestsPending}
                    </td>
                    <td className="px-4 py-2 border-b">
                      {activity.requestsRejected}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      ) : (
        <p>Loading dashboard data...</p>
      )}
    </div>
  );
};

export default DashboardPage;
