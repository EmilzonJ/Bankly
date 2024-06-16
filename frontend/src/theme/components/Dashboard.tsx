import { useGetCustomersQuery } from "@/core/features/customers/api/customers-api.slice";
import { PageContainer } from "@ant-design/pro-components";

function Dashboard() {
  const { data: customers } = useGetCustomersQuery();

  return (
    <PageContainer title="Dashboard">
      <h1>Bankly App</h1>
      <ul>
        {customers?.items.map((customer) => (
          <li key={customer.id}>{customer.name}</li>
        ))}
      </ul>
    </PageContainer>
  );
}

export default Dashboard;
