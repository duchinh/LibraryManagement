const UnauthorizedPage = () => (
  <div className="text-center mt-20 text-red-600 text-xl font-semibold">
    You do not have permission to access this page.
    <button>
      <a href="/" className="text-blue-500 hover:underline ml-2">
        Back to Home Page
      </a>
    </button>
  </div>
);

export default UnauthorizedPage;
