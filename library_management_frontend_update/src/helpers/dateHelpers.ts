export const formatDate = (dateStr: string | null) => {
  if (!dateStr || dateStr === "0001-01-01T00:00:00") return "Pending";
  const date = new Date(dateStr);
  return date.toLocaleDateString("en-US");
};
