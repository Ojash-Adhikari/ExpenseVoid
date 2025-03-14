window.initializeDateRangePicker = () => {
    const dotNetInstance = DotNet.createJSObjectReference({
        SetDateRange: (start, end) => {
            const startDate = new Date(start);
            const endDate = new Date(end);
            dotNetInstance.invokeMethodAsync("SetDateRange", startDate, endDate);
        }
    });

    flatpickr("#dateRangePicker", {
        mode: "range",
        dateFormat: "Y-m-d",
        onChange: function (selectedDates) {
            if (selectedDates.length === 2) {
                dotNetInstance.invokeMethodAsync(
                    "SetDateRange",
                    selectedDates[0].toISOString(),
                    selectedDates[1].toISOString()
                );
            }
        }
    });
};
