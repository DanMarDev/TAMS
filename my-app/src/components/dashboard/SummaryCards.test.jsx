import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import SummaryCards from './SummaryCards';

describe('SummaryCards', () => {
  it('renders all four metric labels', () => {
    render(<SummaryCards summary={{}} />);

    expect(screen.getByText('Total items')).toBeInTheDocument();
    expect(screen.getByText('Estimated value')).toBeInTheDocument();
    expect(screen.getByText('Maybe sell')).toBeInTheDocument();
    expect(screen.getByText('Warranties expiring')).toBeInTheDocument();
  });

  it('formats total estimated value as USD currency', () => {
    render(
      <SummaryCards
        summary={{
          totalItems: 5,
          totalEstimatedValue: 1234.5,
          maybeSellCount: 2,
          expiringWarrantyCount: 1,
        }}
      />
    );

    expect(screen.getByText('$1,234.50')).toBeInTheDocument();
    expect(screen.getByText('5')).toBeInTheDocument();
    expect(screen.getByText('2')).toBeInTheDocument();
    expect(screen.getByText('1')).toBeInTheDocument();
  });

  it('falls back to zeros when summary is undefined', () => {
    render(<SummaryCards summary={undefined} />);

    expect(screen.getByText('$0.00')).toBeInTheDocument();
    // Three zero values: totalItems, maybeSell, expiring
    expect(screen.getAllByText('0')).toHaveLength(3);
  });
});
